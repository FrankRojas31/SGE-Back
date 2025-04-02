using Base.API.Controllers;
using Base.Application.Services.Interfaces.Contrato.Escuela;
using Base.Domain.DTOs.Escuela;
using Base.Domain.Entidades.Escuela;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Base.Controllers.Escuela
{
    [Authorize(Roles = "ADMIN, SERVICIOS ESCOLARES")]
    [Route("api/[controller]")]
    [ApiController]
    public class CursoEscolarController : APIControllerBase<CursoEscolarEntity, CursoEscolarEntityDTO>
    {
        private readonly ICursoEscolarServices _cursoEscolarServices;
        public CursoEscolarController(ICursoEscolarServices cursoEscolarServices) : base(cursoEscolarServices)
        {
            _cursoEscolarServices = cursoEscolarServices;
        }
    }
}
