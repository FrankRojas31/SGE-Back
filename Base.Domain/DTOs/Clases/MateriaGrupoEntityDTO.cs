using Base.Domain.DTO.Core;
using Base.Domain.DTOs.Core;

namespace Base.Domain.DTOs.Clases
{
    public class MateriaGrupoEntityDTO : BaseDTO
    {
        public int IdMateria { get; set; }
        public int IdGrupo { get; set; }
    }
}
