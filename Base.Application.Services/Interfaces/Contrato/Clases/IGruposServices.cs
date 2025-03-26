using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.ViewModels;

namespace Base.Application.Services.Interfaces.Contrato.Clases
{
    public interface IGruposServices : IServiceBase<GruposEntity, GruposEntityDTO>
    {
        Task<ResponseHelper> PostGrupoEnPeriodo(GruposEntityDTO grupo);
        Task<ResponseHelper> GetGruposEnPeriodo();
    }
}
