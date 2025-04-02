using AutoMapper;
using Base.Application.Services.Interfaces.Contrato.Clases;
using Base.Domain.DTO.Security;
using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.Entidades.Escuela;
using Base.Domain.ViewModels;
using Base.Infraestructura.Data.Repositorios.Contrato.Clases;
using Base.Infraestructura.Data.Repositorios.Contrato.Escuela;
using Base.Infraestructura.Data.Repositorios.Contrato.Seguridad;
using static Base.Common.Enumeraciones.Enums;

namespace Base.Application.Services.Interfaces.Implementacion.Clases
{
    public class GruposServices : ServiceBase<GruposEntity, GruposEntityDTO>, IGruposServices
    {
        private readonly IGruposRepository _gruposRepository;
        private readonly IPeriodosRepository _periodosRepository;
        private readonly IGruposPeriodosRepository _gruposPeriodosRepository;
        private readonly IAccountRepository _accountRepository;
        public GruposServices(IMapper mapper, IGruposRepository gruposRepository, IPeriodosRepository periodosRepository, IGruposPeriodosRepository gruposPeriodosRepository, IAccountRepository accountRepository) : base(mapper, gruposRepository)
        {
            _gruposRepository = gruposRepository;
            _periodosRepository = periodosRepository;
            _gruposPeriodosRepository = gruposPeriodosRepository;
            _accountRepository = accountRepository;
        }

        public async Task<ResponseHelper> GetGruposEnPeriodo(string idUsuario)
        {
            try
            {
                PeriodosEntity periodo = await _periodosRepository.GetSingleAsync(x => x.EsBorrado == false && x.EstatusPeriodo == EstatusPeriodo.ACTIVO);

                if (periodo == null)
                    return new ResponseHelper
                    {
                        Success = false,
                        Message = "No existe un periodo activo",
                        Data = Array.Empty<PeriodosEntity>()
                    };

                List<GruposPeriodosEntity> gruposPeriodo = await _gruposPeriodosRepository.GetAllAsync(x => x.EsBorrado == false && x.IdPeriodo == periodo.Id);
                List<GruposEntity> listaGrupos = [];

                if (periodo is null)
                    return new ResponseHelper
                    {
                        Success = true,
                        Message = "¡Lista de Grupos servida Correctamente!",
                        Data = listaGrupos
                    };

                UserDTO usuario = await _accountRepository.GetDTOByIdAsync(idUsuario);
                
                if(usuario.Rol == "ADMIN" || usuario.Rol == "SERVICIOS ESCOLARES")
                {
                    List<GruposEntity> grupos = await _gruposRepository.GetAllAsync(x => x.EsBorrado == false);
                    foreach (GruposEntity grupo in grupos)
                        if (gruposPeriodo.Exists(x => x.IdGrupo == grupo.Id))
                            listaGrupos.Add(new GruposEntity
                            {
                                Id = grupo.Id,
                                Nombre = grupo.Nombre,
                                Descripcion = grupo.Descripcion,
                                EsBorrado = grupo.EsBorrado
                            });
                        else
                            continue;
                }
                else
                {
                    List<GruposEntity> grupos = await _gruposRepository.GetAllAsync(x => x.EsBorrado == false && x.IdUsuario == idUsuario);
                    foreach (GruposEntity grupo in grupos)
                        if (gruposPeriodo.Exists(x => x.IdGrupo == grupo.Id))
                            listaGrupos.Add(new GruposEntity
                            {
                                Id = grupo.Id,
                                Nombre = grupo.Nombre,
                                Descripcion = grupo.Descripcion,
                                EsBorrado = grupo.EsBorrado
                            });
                        else
                            continue;
                }

                return new ResponseHelper
                {
                    Success = true,
                    Message = "¡Lista de Grupos servida Correctamente!",
                    Data = listaGrupos
                };
            }
            catch (Exception ex)
            {
                return new ResponseHelper
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        } 

        public async Task<ResponseHelper> PostGrupoEnPeriodo(GruposEntityDTO grupo)
        {
            try
            {
                PeriodosEntity periodo = await _periodosRepository.GetSingleAsync(x => x.EstatusPeriodo == EstatusPeriodo.ACTIVO && x.EsBorrado == false);

                int idGrupo = await _gruposRepository.InsertAsync(new GruposEntity
                {
                    IdUsuario = grupo.IdUsuario,
                    Nombre = grupo.Nombre,
                    Descripcion = grupo.Descripcion,
                });

                int idGrupoPeriodo = await _gruposPeriodosRepository.InsertAsync(new GruposPeriodosEntity
                {
                    IdGrupo = idGrupo,
                    IdPeriodo = periodo.Id,
                });

                return new ResponseHelper
                {
                    Success = true,
                    Message = "Grupo Insertado Correctamente",
                };
            }
            catch (Exception ex)
            {
                return new ResponseHelper
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
