using Base.API.Controllers;
using Base.Application.Services.Interfaces.Contrato.Personas;
using Base.Domain.DTOs.Personas;
using Base.Domain.Entidades.Personas;
using Base.Domain.ViewModels;
using Base.Domain.ViewModels.Personas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Base.Controllers.Personas
{
    [Authorize(Roles = "ADMIN, SERVICIOS ESCOLARES")]
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoController : APIControllerBase<AlumnoEntity, AlumnoEntityDTO>
    {
        private readonly IAlumnoServices _alumnoServices;
        public AlumnoController(IAlumnoServices alumnoServices) : base(alumnoServices)
        {
            _alumnoServices = alumnoServices;
        }

        [HttpGet("GetAlumnosDatosCompletos")]
        public async Task<ActionResult<List<AlumnoPersonaVM>>> GetAlumnosDatosCompletos()
        {
            var result = await _alumnoServices.GetAlumnosDatosCompletos();
            return Ok(result);
        }

        [HttpGet("GetAlumnoDatosCompletos/{id}")]
        public async Task<ActionResult<AlumnoPersonaVM>> GetAlumnoDatosCompletos(int id)
        {
            var result = await _alumnoServices.GetAlumnoDatosCompletos(id);
            return Ok(result);
        }
    }
}
