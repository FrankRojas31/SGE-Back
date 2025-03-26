using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.ViewModels;

namespace Base.Application.Services.Interfaces.Contrato.Clases
{
    public interface IUnidadesServices : IServiceBase<UnidadesEntity, UnidadesEntityDTO>
    {
        Task<ResponseHelper> GetUnidadesDeGrupo(int id);
    }
}
