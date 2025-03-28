using Base.Domain.DTO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.DTOs.Core
{
    public class ErrorLogsDTO : BaseDTO
    {
        public int IdUsuario { get; set; }
        public string Error { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
