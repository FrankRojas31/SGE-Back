using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.ViewModels;

namespace Base.Application.Services.Interfaces.Contrato.Clases
{
    public interface IMateriaGrupoServices : IServiceBase<MateriaGrupoEntity, MateriaGrupoEntityDTO>
    {
        Task<ResponseHelper> GetMateriaDeGrupo(int id);
        Task<ResponseHelper> GetMateriasNoEnGrupo(int id);
        Task<ResponseHelper> PostMateriasAGrupo(int idGrupo, List<int> idsMaterias);
        Task<ResponseHelper> DeleteMateriaaGrupo(int idGrupo, List<int> idsMaterias);
    }
}
