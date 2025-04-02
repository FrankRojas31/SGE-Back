using Base.API.Controllers;
using Base.Application.Services.Interfaces.Contrato.Clases;
using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Base.Controllers.Clases
{
    [Authorize(Roles = "ADMIN, SERVICIOS ESCOLARES")]
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaGrupoController : APIControllerBase<MateriaGrupoEntity, MateriaGrupoEntityDTO>
    {
        private readonly IMateriaGrupoServices _materiaGrupoServices;
        public MateriaGrupoController(IMateriaGrupoServices materiaGrupoServices) : base(materiaGrupoServices)
        {
            _materiaGrupoServices = materiaGrupoServices;
        }

        [HttpGet("GetMateriaDeGrupo/{id}")]
        public async Task<ActionResult> GetMateriasDeGrupo(int id)
        {
            ResponseHelper response = await _materiaGrupoServices.GetMateriaDeGrupo(id);
            return Ok(response);
        }

        [HttpGet("GetMateriaNoEnGrupo/{id}")]
        public async Task<ActionResult> GetMateriasNoEnGrupo(int id)
        {
            ResponseHelper response = await _materiaGrupoServices.GetMateriasNoEnGrupo(id);
            return Ok(response);
        }

        [HttpPost("PostMateriaAGrupo/{id}")]
        public async Task<ActionResult> PostMateriaAGrupo(int id, [FromBody] List<int> idsMaterias)
        {
            ResponseHelper response = await _materiaGrupoServices.PostMateriasAGrupo(id, idsMaterias);
            return Ok(response);
        }

        [HttpDelete("DeleteMateriaAGrupo/{id}")]
        public async Task<ActionResult> DeleteMateriaAGrupo(int id, [FromBody] List<int> idsMaterias)
        {
            ResponseHelper response = await _materiaGrupoServices.DeleteMateriaaGrupo(id, idsMaterias);
            return Ok(response);
        }

    }
}
