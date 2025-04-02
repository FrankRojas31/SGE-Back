using Base.Domain.Entidades.Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entidades.Core
{
    [Table("Tbl_ErrorLogs")]
    public class ErrorLogs : BaseEntity
    {
        public string? IdUsuario { get; set; }
        public string Error { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
