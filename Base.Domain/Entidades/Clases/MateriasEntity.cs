using Base.Domain.Entidades.Core;
using Base.Domain.Entidades.Escuela;
using Base.Domain.Entidades.Seguridad;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Domain.Entidades.Clases
{
    [Table("Tbl_Materias")]
    public class MateriasEntity : NombreEntity
    {
        // Relaciones
        public virtual ICollection<UnidadesEntity> Unidades { get; set; }
        public virtual ICollection<MateriaGrupoEntity> MateriaGrupos { get; set; }
    }
}
