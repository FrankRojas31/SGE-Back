using Base.Domain.Entidades.Seguridad;
using Base.Infraestructura.Data.Repositorios.Contrato.Seguridad;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using Base.Infraestructura.Data.Repositories.Implementation;
using Base.Infraestructura.Datos.ContextoBD;

namespace Base.Infraestructura.Data.Repositorios.Implementacion.Seguridad
{
    public class ApplicationUserRepository : BaseRepository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly DataBaseContext _context;

        public ApplicationUserRepository(DataBaseContext context, ClaimsPrincipal user) : base(context, user)
        {
            _context = context;
        }

        public async Task<string> GenerateRefreshToken()
        {
            const int maxAttempts = 5;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                var randomNumber = new byte[32];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                string token = Convert.ToBase64String(randomNumber);

                string query = "SELECT COUNT(*) FROM [AspNetUsers] WHERE RefreshToken = @token";
                var count = await _context.Database.GetDbConnection()
                    .QuerySingleAsync<int>(query, new { token });

                if (count == 0)
                {
                    return token;
                }

                attempts++;
            }

            throw new InvalidOperationException("Unable to generate unique refresh token after maximum attempts");
        }
    }
}
