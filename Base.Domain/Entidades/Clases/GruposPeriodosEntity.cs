using Base.Domain.Entidades.Core;
using Base.Domain.Entidades.Escuela;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Domain.Entidades.Clases
{
    [Table("Tbl_GruposPeriodos")]
    public class GruposPeriodosEntity : BaseEntity
    {
        [ForeignKey(nameof(Grupo))]
        public int IdGrupo { get; set; }
        public virtual GruposEntity Grupo { get; set; }

        [ForeignKey(nameof(Periodo))]
        public int IdPeriodo { get; set; }
        public virtual PeriodosEntity Periodo { get; set; }

    }
}
