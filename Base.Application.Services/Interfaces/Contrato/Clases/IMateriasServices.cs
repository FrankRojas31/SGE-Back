using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.ViewModels;

namespace Base.Application.Services.Interfaces.Contrato.Clases
{
    public interface IMateriasServices : IServiceBase<MateriasEntity, MateriasEntityDTO>
    {
        Task<ResponseHelper> GetUnidadesDeMateria(int id);
    }
}
