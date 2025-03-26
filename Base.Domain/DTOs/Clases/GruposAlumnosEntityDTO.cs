using Base.Domain.DTO.Core;
using Base.Domain.DTOs.Core;

namespace Base.Domain.DTOs.Clases
{
    public class GruposAlumnosEntityDTO : BaseDTO
    {
        public int IdGrupoPeriodo { get; set; }
        public int IdAlumno { get; set; }
    }
}
