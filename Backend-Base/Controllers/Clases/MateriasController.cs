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
    public class MateriasController : APIControllerBase<MateriasEntity, MateriasEntityDTO>
    {
        private readonly IMateriasServices _materiasServices;
        public MateriasController(IMateriasServices materiasServices) : base(materiasServices)
        {
            _materiasServices = materiasServices;
        }

        [HttpGet("GetUnidadesDeMateria/{id}")]
        public async Task<ActionResult> GetUnidadesDeMateria(int id)
        {
            ResponseHelper response = await _materiasServices.GetUnidadesDeMateria(id);
            return Ok(response);
        }
    }
}
