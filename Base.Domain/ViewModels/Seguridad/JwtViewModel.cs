
namespace Base.Domain.ViewModels.Seguridad
{
    public class JwtViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public string Role { get; set; }
    }
}
