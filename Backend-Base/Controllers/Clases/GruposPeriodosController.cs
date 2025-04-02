using Base.API.Controllers;
using Base.Application.Services.Interfaces.Contrato.Clases;
using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Base.Controllers.Clases
{
    [Authorize(Roles = "ADMIN, SERVICIOS ESCOLARES")]
    [Route("api/[controller]")]
    [ApiController]
    public class GruposPeriodosController : APIControllerBase<GruposPeriodosEntity, GruposPeriodosEntityDTO>
    {
        private readonly IGruposPeriodosServices _gruposPeriodosService;
        public GruposPeriodosController(IGruposPeriodosServices gruposPeriodosService) : base(gruposPeriodosService)
        {
            _gruposPeriodosService = gruposPeriodosService;
        }
    }
}
