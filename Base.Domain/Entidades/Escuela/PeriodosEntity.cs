using Base.Domain.Entidades.Clases;
using Base.Domain.Entidades.Core;
using Base.Domain.Entidades.Seguridad;
using System.ComponentModel.DataAnnotations.Schema;
using static Base.Common.Enumeraciones.Enums;

namespace Base.Domain.Entidades.Escuela
{
    [Table("Tbl_Periodos")]
    public class PeriodosEntity : NombreEntity
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public EstatusPeriodo EstatusPeriodo { get; set; }

        // Relaciones
        public virtual ICollection<GruposPeriodosEntity> GruposPeriodos { get; set; }
    }
}
