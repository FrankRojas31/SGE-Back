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
using static Base.Common.Enumeraciones.Enums;

namespace Base.Application.Services.Interfaces.Implementacion.Clases
{
    public class GruposAlumnosServices : ServiceBase<GruposAlumnosEntity, GruposAlumnosEntityDTO>, IGruposAlumnosServices
    {
        private readonly IGruposAlumnosRepository _gruposAlumnosRepository;
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly ICursoEscolarRepository _cursoEscolarRepository;
        private readonly IGruposRepository _gruposRepository;
        private readonly IPeriodosRepository _periodosRepository;
        private readonly IGruposPeriodosRepository _gruposPeriodosRepository;
        public GruposAlumnosServices(IMapper mapper, IGruposAlumnosRepository gruposAlumnosRepository, IAlumnoRepository alumnoRepository, IPersonaRepository personaRepository, ICursoEscolarRepository cursoEscolarRepository, IGruposRepository gruposRepository, IGruposPeriodosRepository gruposPeriodosRepository, IPeriodosRepository periodosRepository) : base(mapper, gruposAlumnosRepository)
        {
            _gruposAlumnosRepository = gruposAlumnosRepository;
            _alumnoRepository = alumnoRepository;
            _personaRepository = personaRepository;
            _cursoEscolarRepository = cursoEscolarRepository;
            _gruposRepository = gruposRepository;
            _gruposPeriodosRepository = gruposPeriodosRepository;
            _periodosRepository = periodosRepository;
        }

        public async Task<ResponseHelper> PostAlumnoEnGrupo(int idGrupo, List<int> idsAlumnos)
        {
            try
            {
                PeriodosEntity periodo = await _periodosRepository.GetSingleAsync(x => x.EsBorrado == false && x.EstatusPeriodo == EstatusPeriodo.ACTIVO);
                GruposEntity grupo = await _gruposRepository.GetSingleAsync(x => x.Id == idGrupo);
                GruposPeriodosEntity grupoPeriodo = await _gruposPeriodosRepository.GetSingleAsync(x => x.EsBorrado == false && x.IdGrupo == grupo.Id && x.IdPeriodo == periodo.Id);
                List<GruposAlumnosEntity> grupoAlumnos = await _gruposAlumnosRepository.GetGrupoAlumnosEnPeriodo(periodo.Id);

                if (grupo is not null)
                    foreach (int id in idsAlumnos)
                        if (grupoAlumnos.Any(x => x.IdAlumno == id))
                            continue;
                        else
                            await _gruposAlumnosRepository.InsertAsync(new GruposAlumnosEntity
                            {
                                IdAlumno = id,
                                IdGrupoPeriodo = grupoPeriodo.Id
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
                PeriodosEntity periodo = await _periodosRepository.GetSingleAsync(x => x.EstatusPeriodo == EstatusPeriodo.ACTIVO);
                List<GruposAlumnosEntity> listaAlumnosEnGrupo = await _gruposAlumnosRepository.GetGrupoAlumnosEnPeriodo(periodo.Id);
                List<AlumnoEntity> listaAlumnos = await _alumnoRepository.GetAllAsync(x => x.EsBorrado == false);
                List<AlumnoPersonaVM> listaAlumnosSinGrupo = [];
                foreach (AlumnoEntity alumno in listaAlumnos)
                {
                    if(listaAlumnosEnGrupo.Any(x => x.IdAlumno == alumno.Id))
                        continue;
                    
                    
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
                PeriodosEntity periodo = await _periodosRepository.GetSingleAsync(x => x.EsBorrado == false && x.EstatusPeriodo == EstatusPeriodo.ACTIVO);
                List<GruposAlumnosEntity> listaGrupoAlumnos = await _gruposAlumnosRepository.GetGrupoAlumnosEnPeriodoYGrupoId(periodo.Id, id);
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

        public async Task<ResponseHelper> DeleteAlumnosEnGrupo(int idGrupo, List<int> idsAlumnos)
        {
            try
            {
                PeriodosEntity periodo = await _periodosRepository.GetSingleAsync(x => x.EsBorrado == false && x.EstatusPeriodo == EstatusPeriodo.ACTIVO);
                GruposEntity grupo = await _gruposRepository.GetSingleAsync(x => x.Id == idGrupo);
                GruposPeriodosEntity grupoPeriodo = await _gruposPeriodosRepository.GetSingleAsync(x => x.EsBorrado == false && x.IdGrupo == grupo.Id && x.IdPeriodo == periodo.Id);
                List<GruposAlumnosEntity> grupoAlumnos = await _gruposAlumnosRepository.GetGrupoAlumnosEnPeriodo(periodo.Id);

                if (grupo is not null)
                {
                    foreach (int id in idsAlumnos)
                    {
                        if (grupoAlumnos.Any(x => x.IdAlumno == id))
                        {
                            GruposAlumnosEntity grupoAlumno = grupoAlumnos.FirstOrDefault(x => x.IdAlumno == id);
                            await _gruposAlumnosRepository.RemoveAsync(grupoAlumno);
                        }
                    }

                    return new ResponseHelper
                    {
                        Success = true,
                        Message = "Alumnos eliminados del grupo correctamente."
                    };
                }
                else
                {
                    return new ResponseHelper
                    {
                        Success = false,
                        Message = "¡Cuidado, no existe el grupo a donde quieres ingresar al alumno!"
                    };
                }
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
