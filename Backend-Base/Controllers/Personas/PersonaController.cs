using Base.API.Controllers;
using Base.Application.Services.Interfaces.Contrato.Personas;
using Base.Domain.DTOs.Personas;
using Base.Domain.Entidades.Personas;
using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Base.Controllers.Personas
{
    [Authorize(Roles ="ADMIN, SERVICIOS ESCOLARES")]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : APIControllerBase<PersonaEntity, PersonaEntityDTO>
    {
        private readonly IPersonaServices _personaServices;
        public PersonaController(IPersonaServices personaServices) : base(personaServices)
        {
            _personaServices = personaServices;
        }

        [HttpGet("GetPersonaSinAlumno")]
        public async Task<ActionResult<PersonaEntity>> GetPersonaSinAlumno()
        {
            ResponseHelper response = await _personaServices.GetPersonaSinRelacion();
            return Ok(response);
        }
    }
}
