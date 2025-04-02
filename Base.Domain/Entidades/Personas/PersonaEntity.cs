using Base.Domain.Entidades.Core;
using Base.Domain.Entidades.Seguridad;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Domain.Entidades.Personas
{
    [Table("Tbl_Personas")]
    public class PersonaEntity : BaseEntity
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
