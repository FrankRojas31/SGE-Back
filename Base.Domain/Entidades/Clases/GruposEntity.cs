using Base.Domain.Entidades.Core;
using Base.Domain.Entidades.Escuela;
using Base.Domain.Entidades.Seguridad;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Domain.Entidades.Clases
{
    [Table("Tbl_Grupos")]
    public class GruposEntity : NombreEntity
    {
        [ForeignKey(nameof(User))]
        public string IdUsuario { get; set; }
        public virtual ApplicationUser User { get; set; }

        // Relaciones
        public virtual ICollection<GruposAlumnosEntity> GruposAlumnos { get; set; }
        public virtual ICollection<GruposPeriodosEntity> GruposPeriodos { get; set; }
        public virtual ICollection<MateriaGrupoEntity> MateriaGrupos { get; set; }
    }
}
