using AutoMapper;
using Base.Application.Services.Interfaces.Contrato.Personas;
using Base.Domain.DTOs.Personas;
using Base.Domain.Entidades.Escuela;
using Base.Domain.Entidades.Personas;
using Base.Domain.ViewModels;
using Base.Domain.ViewModels.Personas;
using Base.Infraestructura.Data.Repositorios.Contrato.Escuela;
using Base.Infraestructura.Data.Repositorios.Contrato.Personas;
using System.Linq.Expressions;

namespace Base.Application.Services.Interfaces.Implementacion.Personas
{
    public class AlumnoServices : ServiceBase<AlumnoEntity, AlumnoEntityDTO>, IAlumnoServices
    {
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly ICursoEscolarRepository _cursoEscolarRepository;
        public AlumnoServices(IMapper mapper, IAlumnoRepository alumnoRepository, IPersonaRepository personaRepository, ICursoEscolarRepository cursoEscolarRepository) : base(mapper, alumnoRepository)
        {
            _alumnoRepository = alumnoRepository;
            _personaRepository = personaRepository;
            _cursoEscolarRepository = cursoEscolarRepository;
        }

        public async Task<ResponseHelper> GetAlumnosDatosCompletos()
        {
            try
            {
                List<AlumnoEntity> listaAlumnos = await _alumnoRepository.GetAllAsync();
                List<AlumnoPersonaVM> listaNueva = [];

                foreach (AlumnoEntity alumno in listaAlumnos)
                {
                    PersonaEntity personaAlumno = await _personaRepository.GetById(alumno.IdPersona);
                    CursoEscolarEntity cursoEscolarAlumno = await _cursoEscolarRepository.GetById(alumno.IdCursoEscolar);

                    listaNueva.Add(new AlumnoPersonaVM
                    {
                        Id = alumno.Id,
                        NombreCompleto = $"{personaAlumno.Nombre} {personaAlumno.ApellidoPaterno} {personaAlumno.ApellidoMaterno}",
                        CursoEscolar = cursoEscolarAlumno.Nombre,
                        FechaIngreso = alumno.FechaIngreso,
                        Matricula = alumno.Matricula,
                        IdPersona = alumno.IdPersona,
                        IdCursoEscolar = alumno.IdCursoEscolar,
                        NecesidadesEspeciales = alumno.NecesidadesEspeciales,
                        ContactoEmergencia = alumno.ContactoEmergencia,
                    });
                }

                return new ResponseHelper
                {
                    Success = true,
                    Message = "La lista de datos de alumnos servida correctamente",
                    Data = listaNueva,
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

        public async Task<ResponseHelper> GetAlumnoDatosCompletos(int id)
        {
            try
            {
                AlumnoEntity alumno = await _alumnoRepository.GetById(id);
                AlumnoPersonaVM alumnoCompleto = new();

                if (alumno != null)
                {
                    PersonaEntity personaAlumno = await _personaRepository.GetById(alumno.IdPersona);
                    CursoEscolarEntity cursoEscolarAlumno = await _cursoEscolarRepository.GetById(alumno.IdCursoEscolar);
                    alumnoCompleto = new AlumnoPersonaVM
                    {
                        Id = alumno.Id,
                        NombreCompleto = $"{personaAlumno.Nombre} {personaAlumno.ApellidoPaterno} {personaAlumno.ApellidoMaterno}",
                        CursoEscolar = cursoEscolarAlumno.Nombre,
                        FechaIngreso = alumno.FechaIngreso,
                        Matricula = alumno.Matricula,
                        IdPersona = alumno.IdPersona,
                        IdCursoEscolar = alumno.IdCursoEscolar,
                        NecesidadesEspeciales = alumno.NecesidadesEspeciales,
                        ContactoEmergencia = alumno.ContactoEmergencia,
                    };
                }

                return new ResponseHelper
                {
                    Success = true,
                    Message = "Los datos del alumno fueron servidos correctamente",
                    Data = alumnoCompleto,
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
    }
}
