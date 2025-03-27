using Base.Domain.Entidades.Clases;
using Base.Infraestructura.Data.Repositories.Implementation;
using Base.Infraestructura.Data.Repositorios.Contrato.Clases;
using Base.Infraestructura.Datos.ContextoBD;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Base.Infraestructura.Data.Repositorios.Implementacion.Clases
{
    public class MateriasRepository : BaseRepository<MateriasEntity>, IMateriasRepository
    {
        private readonly DataBaseContext _context;
        public MateriasRepository(DataBaseContext context, ClaimsPrincipal user) : base(context, user)
        {
            _context = context;
        }

        public async Task<List<UnidadesEntity>> GetUnidadesDeMateria(int id)
        {
            string query = @"
            SELECT uni.* FROM Tbl_Unidades uni
	            INNER JOIN Tbl_Materias ma ON uni.IdMateria = ma.Id
	                WHERE 
		                ma.Id = @id
	                AND	uni.EsBorrado = @esBorrado
	                AND ma.EsBorrado = @esBorrado";

            var response = await _context.Database.GetDbConnection().QueryAsync<UnidadesEntity>(query, new { id, esBorrado = false });

            return response.ToList();
        }
    }
}
