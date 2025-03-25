using Base.Domain.DTOs.Escuela;
using Base.Domain.Entidades.Escuela;
using Base.Domain.ViewModels;

namespace Base.Application.Services.Interfaces.Contrato.Escuela
{
    public interface IPeriodosServices : IServiceBase<PeriodosEntity, PeriodosEntityDTO>
    {
        Task<ResponseHelper> GetPeriodoActivo();
    }
}
