using Base.Domain.Entidades.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infraestructura.Data.Repositorios.Contrato.Clases
{
    public interface IGruposAlumnosRepository : IBaseRepository<GruposAlumnosEntity>
    {
        Task<List<GruposAlumnosEntity>> GetGrupoAlumnosEnPeriodo(int idPeriodo);
        Task<List<GruposAlumnosEntity>> GetGrupoAlumnosEnPeriodoYGrupoId(int idPeriodo, int idGrupo);
    }
}
