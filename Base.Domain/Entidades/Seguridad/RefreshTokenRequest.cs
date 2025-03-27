using System.ComponentModel.DataAnnotations;

namespace Base.Domain.Entidades.Seguridad
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; }
    }
}
