using Base.Domain.Entidades.Core;
using Base.Domain.Entidades.Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entidades.Escuela
{
    [Table("Tbl_CursosEscolares")]
    public class CursoEscolarEntity : NombreEntity
    {
    }
}
