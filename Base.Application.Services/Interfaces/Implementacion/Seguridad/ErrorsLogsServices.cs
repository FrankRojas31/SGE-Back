using AutoMapper;
using Base.Application.Services.Interfaces.Contrato;
using Base.Application.Services.Interfaces.Contrato.Seguridad;
using Base.Domain.DTOs.Core;
using Base.Domain.Entidades.Core;
using Base.Infraestructura.Data.Repositorios.Contrato;
using Base.Infraestructura.Data.Repositorios.Contrato.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Services.Interfaces.Implementacion.Seguridad
{
    public class ErrorsLogsServices : ServiceBase<ErrorLogs, ErrorLogsDTO>, IErrorsLogsServices
    {
        private readonly IErrorsLogRepository _errosLogRepository;
        public ErrorsLogsServices(IMapper mapper, IErrorsLogRepository errorsLogRepository) : base(mapper, errorsLogRepository)
        {
        }
    }
}
