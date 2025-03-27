using Base.Domain.Entidades.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infraestructura.Data.Repositorios.Contrato.Clases
{
    public interface IMateriasRepository : IBaseRepository<MateriasEntity>
    {
        Task<List<UnidadesEntity>> GetUnidadesDeGrupo(int id);
    }
}
