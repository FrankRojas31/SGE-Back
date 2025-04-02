using Base.API.Controllers;
using Base.Application.Services.Interfaces.Contrato.Clases;
using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Base.Controllers.Clases
{
    [Authorize(Roles = "ADMIN, SERVICIOS ESCOLARES")]
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesController : APIControllerBase<UnidadesEntity, UnidadesEntityDTO>
    {
        private readonly IUnidadesServices _unidadesServices;
        public UnidadesController(IUnidadesServices unidadesServices) : base(unidadesServices)
        {
            _unidadesServices = unidadesServices;
        }

        [HttpGet("GetUnidadesDeGrupo/{id}")]
        public async Task<ActionResult<List<UnidadesEntity>>> GetUnidadesDeGrupo(int id)
        {
            ResponseHelper response = await _unidadesServices.GetUnidadesDeGrupo(id);
            return Ok(response);
        }
    }
}
