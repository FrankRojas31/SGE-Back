using Base.API.Controllers;
using Base.Application.Services.Interfaces.Contrato.Escuela;
using Base.Domain.DTOs.Escuela;
using Base.Domain.Entidades.Escuela;
using Base.Domain.ViewModels;
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

        [HttpPost("PostPeriodo")]
        public async Task<ActionResult> PostPeriodo(PeriodosEntityDTO periodo)
        {
            ResponseHelper response = await _periodosServices.PostPeriodo(periodo);
            return Ok(response);
        }

        [HttpPut("PutPeriodo")]
        public async Task<ActionResult> PutPeriodo(PeriodosEntityDTO periodo)
        {
            ResponseHelper response = await _periodosServices.PutPeriodo(periodo);
            return Ok(response);
        }
    }
}
