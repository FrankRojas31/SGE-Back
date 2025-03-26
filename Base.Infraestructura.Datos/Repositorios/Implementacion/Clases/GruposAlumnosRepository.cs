using Base.Domain.Entidades.Clases;
using Base.Infraestructura.Data.Repositories.Implementation;
using Base.Infraestructura.Data.Repositorios.Contrato.Clases;
using Base.Infraestructura.Datos.ContextoBD;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Base.Infraestructura.Data.Repositorios.Implementacion.Clases
{
    public class GruposAlumnosRepository : BaseRepository<GruposAlumnosEntity>, IGruposAlumnosRepository
    {
        private readonly DataBaseContext _context;
        public GruposAlumnosRepository(DataBaseContext context, ClaimsPrincipal user) : base(context, user)
        {
            _context = context;
        }

        public async Task<List<GruposAlumnosEntity>> GetGrupoAlumnosEnPeriodo(int idPeriodo)
        {
            string query = @"  
            SELECT ga.* FROM Tbl_GruposAlumnos ga 
	            INNER JOIN Tbl_GruposPeriodos gp ON ga.IdGrupoPeriodo = gp.Id
	        WHERE
		        gp.IdPeriodo = @idPeriodo
	        AND ga.EsBorrado = @esBorrado
	        AND gp.EsBorrado = @esBorrado";

            var results = await _context.Database.GetDbConnection().QueryAsync<GruposAlumnosEntity>(query, new { idPeriodo, esBorrado = false });
            return results.ToList();
        }

        public async Task<List<GruposAlumnosEntity>> GetGrupoAlumnosEnPeriodoYGrupoId(int idPeriodo, int idGrupo)
        {
            string query = @"
            SELECT ga.* FROM Tbl_GruposAlumnos ga 
	            INNER JOIN Tbl_GruposPeriodos gp ON ga.IdGrupoPeriodo = gp.Id
	        WHERE
		        gp.IdPeriodo = @idPeriodo
	        AND gp.IdGrupo = @idGrupo
	        AND ga.EsBorrado = @esBorrado
	        AND gp.EsBorrado = @esBorrado";

            var results = await _context.Database.GetDbConnection().QueryAsync<GruposAlumnosEntity>(query, new { idPeriodo, idGrupo, esBorrado = false });
            return results.ToList();
        }
    }
}
