using AutoMapper;
using Base.Application.Services.Interfaces.Contrato.Clases;
using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.Entidades.Escuela;
using Base.Domain.Entidades.Personas;
using Base.Domain.ViewModels;
using Base.Domain.ViewModels.Personas;
using Base.Infraestructura.Data.Repositorios.Contrato.Clases;
using Base.Infraestructura.Data.Repositorios.Contrato.Escuela;
using Base.Infraestructura.Data.Repositorios.Contrato.Personas;

namespace Base.Application.Services.Interfaces.Implementacion.Clases
{
    public class GruposAlumnosServices : ServiceBase<GruposAlumnosEntity, GruposAlumnosEntityDTO>, IGruposAlumnosServices
    {
        private readonly IGruposAlumnosRepository _gruposAlumnosRepository;
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly ICursoEscolarRepository _cursoEscolarRepository;
        private readonly IGruposRepository _gruposRepository;
        public GruposAlumnosServices(IMapper mapper, IGruposAlumnosRepository gruposAlumnosRepository, IAlumnoRepository alumnoRepository, IPersonaRepository personaRepository, ICursoEscolarRepository cursoEscolarRepository) : base(mapper, gruposAlumnosRepository)
        {
            _gruposAlumnosRepository = gruposAlumnosRepository;
            _alumnoRepository = alumnoRepository;
            _personaRepository = personaRepository;
            _cursoEscolarRepository = cursoEscolarRepository;
        }

        public async Task<ResponseHelper> PostAlumnoEnGrupo(int idGrupo, List<int> idsAlumnos)
        {
            try
            {
                GruposEntity grupo = await _gruposRepository.GetById(idGrupo);

                if (grupo is not null)
                    foreach (int id in idsAlumnos)
                        await _gruposAlumnosRepository.InsertAsync(new GruposAlumnosEntity
                        {
                            IdAlumno = id,
                            IdGrupo = idGrupo
                        });
                else
                    return new ResponseHelper
                    {
                        Success = false,
                        Message = "¡Cuidado, no existe el grupo a donde quieres ingresar al alumno!"
                    };

                return new ResponseHelper
                {
                    Success = true,
                    Message = "¡Se han insertado todos los alumnos en el grupo!",
                };
            }
            catch (Exception ex)
            {
                return new ResponseHelper
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<ResponseHelper> GetAlumnosSinGrupo()
        {
            try
            {
                List<GruposAlumnosEntity> listaAlumnosEnGrupo = await _gruposAlumnosRepository.GetAllAsync(x => x.EsBorrado == false);
                List<AlumnoEntity> listaAlumnos = await _alumnoRepository.GetAllAsync(x => x.EsBorrado == false);
                List<AlumnoPersonaVM> listaAlumnosSinGrupo = [];
                foreach (AlumnoEntity alumno in listaAlumnos)
                {
                    if(listaAlumnosEnGrupo.Any(x => x.IdAlumno == alumno.Id))
                    {
                        continue;
                    }
                    
                    PersonaEntity persona = await _personaRepository.GetById(alumno.IdPersona);
                    CursoEscolarEntity curso = await _cursoEscolarRepository.GetById(alumno.IdCursoEscolar);

                    listaAlumnosSinGrupo.Add(new AlumnoPersonaVM
                    {
                        Id = alumno.Id,
                        IdPersona = alumno.IdPersona,
                        IdCursoEscolar = alumno.IdCursoEscolar,
                        ContactoEmergencia = alumno.ContactoEmergencia,
                        CursoEscolar = curso.Nombre,
                        FechaIngreso = alumno.FechaIngreso,
                        Matricula = alumno.Matricula,
                        NombreCompleto = $"{persona.Nombre} {persona.ApellidoMaterno} {persona.ApellidoPaterno}",
                        NecesidadesEspeciales = alumno.NecesidadesEspeciales
                    });
                }

                return new ResponseHelper
                {
                    Message = "¡Lista de Alumnos sin Grupo Servidad Correctamente!",
                    Success = true,
                    Data = listaAlumnosSinGrupo
                };
            }
            catch (Exception ex)
            {
                return new ResponseHelper
                {
                    Message = ex.Message,
                    Success = false,
                };
            }
        }

        public async Task<ResponseHelper> GetAlumnosEnGrupo(int id)
        {
            try
            {
                List<GruposAlumnosEntity> listaGrupoAlumnos = await _gruposAlumnosRepository.GetAllAsync(x => x.EsBorrado == false && x.IdGrupo == id);
                List<AlumnoPersonaVM> listaAlumnosEnGrupo = [];

                foreach (GruposAlumnosEntity grupos in listaGrupoAlumnos)
                {
                    AlumnoEntity alumno = await _alumnoRepository.GetById(grupos.IdAlumno);
                    PersonaEntity persona = await _personaRepository.GetById(alumno.IdPersona);
                    CursoEscolarEntity curso = await _cursoEscolarRepository.GetById(alumno.IdCursoEscolar);

                    listaAlumnosEnGrupo.Add(new AlumnoPersonaVM
                    {
                        Id = alumno.Id,
                        IdPersona = alumno.IdPersona,
                        IdCursoEscolar = alumno.IdCursoEscolar,
                        ContactoEmergencia = alumno.ContactoEmergencia,
                        CursoEscolar = curso.Nombre,
                        FechaIngreso = alumno.FechaIngreso,
                        Matricula = alumno.Matricula,
                        NombreCompleto = $"{persona.Nombre} {persona.ApellidoMaterno} {persona.ApellidoPaterno}",
                        NecesidadesEspeciales = alumno.NecesidadesEspeciales
                    });
                }

                return new ResponseHelper
                {
                    Success = true,
                    Message = "¡Lista de Alumnos en el Grupo servida correctamente!",
                    Data = listaAlumnosEnGrupo
                };
            }
            catch (Exception ex)
            {
                return new ResponseHelper
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
