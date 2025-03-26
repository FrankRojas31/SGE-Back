using Base.Domain.Entidades.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entidades.Clases
{
    [Table("Tbl_MateriasGrupos")]
    public class MateriaGrupoEntity : BaseEntity
    {
        [ForeignKey(nameof(Materia))]
        public int IdMateria { get; set; }
        public virtual MateriasEntity Materia { get; set; }

        [ForeignKey(nameof(Grupo))]
        public int IdGrupo { get; set; }
        public virtual GruposEntity Grupo { get; set; }
    }
}
