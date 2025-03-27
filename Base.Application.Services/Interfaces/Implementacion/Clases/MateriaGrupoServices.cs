using AutoMapper;
using Base.Application.Services.Interfaces.Contrato.Clases;
using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.ViewModels;
using Base.Infraestructura.Data.Repositorios.Contrato.Clases;

namespace Base.Application.Services.Interfaces.Implementacion.Clases
{
    public class MateriaGrupoServices : ServiceBase<MateriaGrupoEntity, MateriaGrupoEntityDTO>, IMateriaGrupoServices
    {
        private readonly IMateriaGrupoRepository _materiaGrupoRepository;
        private readonly IMateriasRepository _materiasRepository;
        private readonly IGruposRepository _gruposRepository;
        public MateriaGrupoServices(IMapper mapper, IMateriaGrupoRepository materiaGrupoRepository, IMateriasRepository materiasRepository, IGruposRepository gruposRepository) : base(mapper, materiaGrupoRepository)
        {
            _materiaGrupoRepository = materiaGrupoRepository;
            _materiasRepository = materiasRepository;
            _gruposRepository = gruposRepository;
        }

        public async Task<ResponseHelper> GetMateriaDeGrupo(int id)
        {
            try
            {
                List<MateriaGrupoEntity> materiaGrupo = await _materiaGrupoRepository.GetAllAsync(x => x.IdGrupo == id && x.EsBorrado == false);
                List<MateriasEntity> listaMaterias = [];

                foreach (MateriaGrupoEntity materia in materiaGrupo)
                    if (materiaGrupo.Any(x => x.IdMateria == materia.IdMateria))
                    {
                        MateriasEntity localMateria = await _materiasRepository.GetSingleAsync(x => x.Id == materia.IdMateria);
                        listaMaterias.Add(new MateriasEntity
                        {
                            Id = localMateria.Id,
                            Nombre = localMateria.Nombre,
                            Descripcion = localMateria.Descripcion,
                            EsBorrado = localMateria.EsBorrado,
                        });
                    }
                    else
                    {
                        continue;
                    }
                return new ResponseHelper
                {
                    Success = true,
                    Data = listaMaterias,
                    Message = "¡Lista de Materias en el Grupo servida correctamente!"
                };
            }
            catch (Exception ex)
            {
                return new ResponseHelper
                {
                    Success = false,
                    Data = ex.Message
                };
            }
        }

        public async Task<ResponseHelper> GetMateriasNoEnGrupo(int id)
        {
            try
            {
                List<MateriasEntity> materiasofertadas = await _materiasRepository.GetAllAsync(x => x.EsBorrado == false);
                List<MateriaGrupoEntity> materiaGrupo = await _materiaGrupoRepository.GetAllAsync(x => x.IdGrupo == id && x.EsBorrado == false);
                List<MateriasEntity> listaMaterias = [];

                foreach (MateriasEntity materia in materiasofertadas)
                    if (materiaGrupo.Any(x => x.IdMateria == materia.Id))
                    {
                        continue;
                    }
                    else
                    {
                        listaMaterias.Add(new MateriasEntity
                        {
                            Id = materia.Id,
                            Nombre = materia.Nombre,
                            Descripcion = materia.Descripcion,
                            EsBorrado = materia.EsBorrado,
                        });
                    }
                return new ResponseHelper
                {
                    Success = true,
                    Data = listaMaterias,
                    Message = "¡Lista de Materias en el Grupo servida correctamente!"
                };
            }
            catch (Exception ex)
            {
                return new ResponseHelper
                {
                    Success = false,
                    Data = ex.Message
                };
            }
        }

        public async Task<ResponseHelper> PostMateriasAGrupo(int idGrupo, List<int> idsMaterias)
        {
            try
            {
                GruposEntity grupos = await _gruposRepository.GetSingleAsync(x => x.Id == idGrupo && x.EsBorrado == false);

                if (grupos is not null)
                {
                    foreach (int id in idsMaterias)
                    {
                        MateriasEntity materia = await _materiasRepository.GetSingleAsync(x => x.Id == id);
                        if (materia is not null)
                            await _materiaGrupoRepository.InsertAsync(new MateriaGrupoEntity
                            {
                                IdGrupo = idGrupo,
                                IdMateria = id,
                                EsBorrado = false
                            });
                        else
                            continue;   
                    }

                    return new ResponseHelper
                    {
                        Success = true,
                        Message = "¡Lista de Materias Agregada Correctamente"
                    };
                } 
                else
                {
                    return new ResponseHelper
                    {
                        Success = false,
                        Message = "¡Cuidado al Grupo donde deseas Insertar! Este Grupo no existe!"
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResponseHelper
                {
                    Success = false,
                    Data = ex.Message
                };
            }
        } 

        public async Task<ResponseHelper> DeleteMateriaaGrupo(int idGrupo, List<int> idsMaterias)
        {
            try
            {
                List<MateriaGrupoEntity> materiaGrupo = await _materiaGrupoRepository.GetAllAsync(x => x.IdGrupo == idGrupo && x.EsBorrado == false);
                GruposEntity grupos = await _gruposRepository.GetSingleAsync(x => x.Id == idGrupo && x.EsBorrado == false);

                if (grupos is not null)
                    foreach (int id in idsMaterias)
                    {
                        MateriasEntity materia = await _materiasRepository.GetSingleAsync(x => x.Id == id);
                        if (materia is not null)
                            if (materiaGrupo.Any(x => x.IdMateria == id))
                                await _materiaGrupoRepository.RemoveAsync(materiaGrupo.FirstOrDefault(x => x.IdMateria == id));
                            else
                                continue;
                        else
                            return new ResponseHelper
                            {
                                Success = true,
                                Message = "¡Cuidado, estas intentando borrar una materia que no existe!"
                            };
                    }
                else
                    return new ResponseHelper
                    {
                        Success = false,
                        Message = "¡Cuidado, estas intentando insertar materias en un grupo que no existe!"
                    };

                return new ResponseHelper
                {
                    Success = true,
                    Message = "¡Se han borrado las materias en este grupo!"
                };
            }
            catch (Exception ex)
            {
                return new ResponseHelper
                {
                    Success = false,
                    Data = ex.Message
                };
            }
        }
    }
}
