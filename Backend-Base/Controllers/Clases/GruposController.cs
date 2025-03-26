using Base.API.Controllers;
using Base.Application.Services.Interfaces.Contrato.Clases;
using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Base.Controllers.Clases
{
    [Route("api/[controller]")]
    [ApiController]
    public class GruposController : APIControllerBase<GruposEntity, GruposEntityDTO>
    {
        private readonly IGruposServices _gruposServices;
        public GruposController(IGruposServices gruposServices) : base(gruposServices)
        {
            _gruposServices = gruposServices;
        }

        [HttpGet("GetGruposEnPeriodo")]
        public async Task<IActionResult> GetGruposEnPeriodo()
        {
            ResponseHelper response = await _gruposServices.GetGruposEnPeriodo();
            return Ok(response);
        }

        [HttpPost("PostGrupoEnPeriodo")]
        public async Task<IActionResult> PostGrupoEnPeriodo(GruposEntityDTO request)
        {
            ResponseHelper response = await _gruposServices.PostGrupoEnPeriodo(request);
            return Ok(response);
        }
    }
}
