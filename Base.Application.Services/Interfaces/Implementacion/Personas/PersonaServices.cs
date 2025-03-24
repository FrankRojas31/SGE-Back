using AutoMapper;
using Base.Application.Services.Interfaces.Contrato.Personas;
using Base.Domain.DTOs.Personas;
using Base.Domain.Entidades.Personas;
using Base.Domain.ViewModels;
using Base.Infraestructura.Data.Repositorios.Contrato;
using Base.Infraestructura.Data.Repositorios.Contrato.Personas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Services.Interfaces.Implementacion.Personas
{
    public class PersonaServices : ServiceBase<PersonaEntity, PersonaEntityDTO>, IPersonaServices
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly IAlumnoRepository _alumnoRepository;
        public PersonaServices(IMapper mapper, IPersonaRepository personaRepository, IAlumnoRepository alumnoRepository) : base(mapper, personaRepository)
        {
            _personaRepository = personaRepository;
            _alumnoRepository = alumnoRepository;
        }

        public async Task<ResponseHelper> GetPersonaSinRelacion()
        {
            try
            {
                List<AlumnoEntity> listaAlumno = await _alumnoRepository.GetAllAsync();
                List<PersonaEntity> listaPersona = await _personaRepository.GetAllAsync();
                List<PersonaEntity> lista = [];

                foreach (PersonaEntity persona in listaPersona)
                {
                    if (listaAlumno.Any(x => x.IdPersona == persona.Id))
                        continue;
                    else
                        lista.Add(persona);
                }

                return new ResponseHelper
                {
                    Success = true,
                    Message = "Lista de Personas servida correctamente",
                    Data = lista
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
