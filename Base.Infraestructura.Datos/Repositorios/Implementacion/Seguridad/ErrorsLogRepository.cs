using Base.Domain.Entidades.Core;
using Base.Infraestructura.Data.Repositories.Implementation;
using Base.Infraestructura.Data.Repositorios.Contrato.Seguridad;
using Base.Infraestructura.Datos.ContextoBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infraestructura.Data.Repositorios.Implementacion.Seguridad
{
    public class ErrorsLogRepository : BaseRepository<ErrorLogs>, IErrorsLogRepository
    {
        public ErrorsLogRepository(DataBaseContext context, ClaimsPrincipal user) : base(context, user)
        {
        }
    }
}
