using Base.Domain.DTOs.Personas;
using Base.Domain.Entidades.Personas;
using Base.Domain.ViewModels;

namespace Base.Application.Services.Interfaces.Contrato.Personas
{
    public interface IPersonaServices : IServiceBase<PersonaEntity, PersonaEntityDTO>
    {
        Task<ResponseHelper> GetPersonaSinRelacion();
    }
}
