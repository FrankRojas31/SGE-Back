using Base.API.Controllers;
using Base.Application.Services.Interfaces.Contrato.Clases;
using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.ViewModels;
using Base.Domain.ViewModels.Personas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Base.Controllers.Clases
{
    [Authorize(Roles = "ADMIN, SERVICIOS ESCOLARES")]
    [Route("api/[controller]")]
    [ApiController]
    public class GruposAlumnosController : APIControllerBase<GruposAlumnosEntity, GruposAlumnosEntityDTO>
    {
        private readonly IGruposAlumnosServices _gruposAlumnosService;
        public GruposAlumnosController(IGruposAlumnosServices gruposAlumnosService) : base(gruposAlumnosService)
        {
            _gruposAlumnosService = gruposAlumnosService;
        }

        [HttpGet("GetAlumnosSinGrupo")]
        public async Task<ActionResult<List<AlumnoPersonaVM>>> GetAlumnosSinGrupo()
        {
            ResponseHelper response = await _gruposAlumnosService.GetAlumnosSinGrupo();
            return Ok(response);
        }

        [HttpGet("GetAlumnosConGrupo/{id}")]
        public async Task<ActionResult<List<AlumnoPersonaVM>>> GetAlumnosEnGrupo(int id)
        {
            ResponseHelper response = await _gruposAlumnosService.GetAlumnosEnGrupo(id);
            return Ok(response);
        }

        [HttpPost("PostAlumnosaGrupo/{id}")]
        public async Task<ActionResult> PostAlumnosaGrupo(int id, [FromBody]List<int> idsAlumnos)
        {
            ResponseHelper response = await _gruposAlumnosService.PostAlumnoEnGrupo(id, idsAlumnos);
            return Ok(response);
        }

        [HttpDelete("DeleteAlumnosEnGrupo/{id}")]
        public async Task<ActionResult> DeleteAlumnosEnGrupo(int id, [FromBody] List<int> idsAlumnos)
        {
            ResponseHelper response = await _gruposAlumnosService.DeleteAlumnosEnGrupo(id, idsAlumnos); 
            return Ok(response);
        }
    }
}
