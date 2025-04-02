using Base.API.Controllers;
using Base.Application.Services.Interfaces.Contrato.Escuela;
using Base.Domain.DTOs.Escuela;
using Base.Domain.Entidades.Escuela;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Base.Controllers.Escuela
{
    [Authorize(Roles ="ADMIN, SERVICIOS ESCOLARES, PROFESOR")]
    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionesController : APIControllerBase<CalificacionesEntity, CalificacionesEntityDTO>
    {
        private readonly ICalificacionesServices _calificacionesServices;
        public CalificacionesController(ICalificacionesServices calificacionesServices) : base(calificacionesServices)
        {
            _calificacionesServices = calificacionesServices;
        }
    }
}
