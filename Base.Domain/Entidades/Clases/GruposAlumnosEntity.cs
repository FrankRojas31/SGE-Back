using Base.Domain.Entidades.Core;
using Base.Domain.Entidades.Personas;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Domain.Entidades.Clases
{
    [Table("Tbl_GruposAlumnos")]
    public class GruposAlumnosEntity : BaseEntity
    {
        [ForeignKey(nameof(GrupoPeriodo))]
        public int IdGrupoPeriodo { get; set; }
        public virtual GruposPeriodosEntity GrupoPeriodo { get; set; }

        [ForeignKey(nameof(Alumno))]
        public int IdAlumno { get; set; }
        public virtual AlumnoEntity Alumno { get; set; }

    }
}
