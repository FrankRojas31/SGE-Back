using Base.API.Controllers;
using Base.Application.Services.Interfaces.Contrato.Escuela;
using Base.Domain.DTOs.Escuela;
using Base.Domain.DTOs.Personas;
using Base.Domain.Entidades.Escuela;
using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Base.Controllers.Escuela
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodosController : APIControllerBase<PeriodosEntity, PeriodosEntityDTO>
    {
        private readonly IPeriodosServices _periodosServices;
        public PeriodosController(IPeriodosServices periodosServices) : base(periodosServices)
        {
            _periodosServices = periodosServices;
        }

        [HttpGet("GetPeriodoActivo")]
        public async Task<ActionResult<PeriodosEntity>> GetPeriodoActivo()
        {
            ResponseHelper response = await _periodosServices.GetPeriodoActivo();
            return Ok(response);
        }
    }
}
