using Base.Application.Services.Interfaces.Contrato;
using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend_Base.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeederController : ControllerBase
    {
        private readonly ISeedersServices _services;
        public SeederController(ISeedersServices services)
        {
            _services = services;
        }

        [HttpGet("SeederInicial")]
        public async Task<ActionResult> SeederInicial()
        {
            return Ok(await _services.Seeders());
        }
    }
}
