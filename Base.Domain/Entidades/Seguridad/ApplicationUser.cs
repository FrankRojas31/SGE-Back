using Base.Domain.Entidades.Personas;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using static Base.Common.Enumeraciones.Enums;

namespace Base.Domain.Entidades.Seguridad
{
    public class ApplicationUser : IdentityUser
    {

        public DateTime RowVersion { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public EstatusUsuario EstatusUsuario { get; set; }
        
        //JWT
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }

}
