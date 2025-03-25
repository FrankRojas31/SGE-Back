using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.ViewModels;

namespace Base.Application.Services.Interfaces.Contrato.Clases
{
    public interface IGruposAlumnosServices : IServiceBase<GruposAlumnosEntity, GruposAlumnosEntityDTO>
    {
        Task<ResponseHelper> GetAlumnosSinGrupo();
        Task<ResponseHelper> GetAlumnosEnGrupo(int id);
        Task<ResponseHelper> PostAlumnoEnGrupo(int idGrupo, List<int> idsAlumnos);
    }
}
