using Base.API.Controllers;
using Base.Application.Services.Interfaces.Contrato;
using Base.Application.Services.Interfaces.Contrato.Seguridad;
using Base.Domain.DTOs.Core;
using Base.Domain.Entidades.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Base.Controllers.Seguridad
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorsLogsController : APIControllerBase<ErrorLogs, ErrorLogsDTO>
    {
        private readonly IErrorsLogsServices _errorsLogsServices;
        public ErrorsLogsController(IErrorsLogsServices errorslogs) : base(errorslogs)
        {
        }
    }
}
