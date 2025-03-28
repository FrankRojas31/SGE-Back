using AutoMapper;
using Base.Application.Services.Interfaces.Contrato.Escuela;
using Base.Domain.DTOs.Escuela;
using Base.Domain.Entidades.Escuela;
using Base.Domain.ViewModels;
using Base.Infraestructura.Data.Repositorios.Contrato.Escuela;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Base.Common.Enumeraciones.Enums;

namespace Base.Application.Services.Interfaces.Implementacion.Escuela
{
    public class PeriodosServices : ServiceBase<PeriodosEntity, PeriodosEntityDTO>, IPeriodosServices
    {
        private readonly IPeriodosRepository _periodosRepository;
        public PeriodosServices(IMapper mapper, IPeriodosRepository periodosRepository) : base(mapper, periodosRepository)
        {
            _periodosRepository = periodosRepository;
        }

        public async Task<ResponseHelper> GetPeriodoActivo()
        {
            try
            {
                PeriodosEntity response = await _periodosRepository.GetSingleAsync(x => x.EsBorrado == false && x.EstatusPeriodo == Common.Enumeraciones.Enums.EstatusPeriodo.ACTIVO);

                return new ResponseHelper
                {
                    Success = true,
                    Message = "¡El Periodo Activo Servido correctamente!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseHelper
                {
                    Message = ex.Message,
                    Success = false,
                };
            }
        }

        public async Task<ResponseHelper> PostPeriodo(PeriodosEntityDTO periodos)
        {
            try
            {
                PeriodosEntity periodo = await _periodosRepository.GetSingleAsync(x => x.EsBorrado == false && x.EstatusPeriodo == EstatusPeriodo.ACTIVO);

                if (periodo is not null && periodo.EstatusPeriodo == EstatusPeriodo.ACTIVO && periodos.EstatusPeriodo == EstatusPeriodo.ACTIVO)
                    return new ResponseHelper {
                        Success = false,
                        Message = "¡Cuidado... No se permite el registro de un nuevo período con Estatus Activo en el sistema!"
                    };

                PeriodosEntity periodoNew = new()
                {
                    Nombre = periodos.Nombre,
                    Descripcion = periodos.Descripcion,
                    FechaInicio = periodos.FechaInicio,
                    FechaFin = periodos.FechaFin,
                    EstatusPeriodo = periodos.EstatusPeriodo
                };

                int response = await _periodosRepository.InsertAsync(periodoNew);

                if (response > 0)
                    return new ResponseHelper
                    {
                        Success = true,
                        Message = "¡Se inserto con exito el periodo!"
                    };

                return new ResponseHelper
                {
                    Success = false,
                    Message = "¡Ups.. No se Inserto el Periodo... Ponte en Contacto con un Administrador!"
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

        public async Task<ResponseHelper> PutPeriodo(PeriodosEntityDTO periodos)
        {
            try
            {
                PeriodosEntity periodo = await _periodosRepository.GetSingleAsync(x => x.EsBorrado == false && x.EstatusPeriodo == EstatusPeriodo.ACTIVO);

                if (periodo is not null && periodo.EstatusPeriodo == EstatusPeriodo.ACTIVO && periodos.EstatusPeriodo == EstatusPeriodo.ACTIVO && periodos.Id != periodo.Id)
                    return new ResponseHelper
                    {
                        Success = false,
                        Message = "¡Cuidado... No se permite el registro de un nuevo período con Estatus Activo en el sistema!"
                    };

                PeriodosEntity periodoNuevo = new()
                {
                    Id = (int)periodos.Id,
                    Nombre = periodos.Nombre,
                    Descripcion = periodos.Descripcion,
                    FechaInicio = periodos.FechaInicio,
                    FechaFin = periodos.FechaFin,
                    EstatusPeriodo = periodos.EstatusPeriodo
                };

                int response = await _periodosRepository.UpdateAsync(periodoNuevo);

                if (response > 0)
                    return new ResponseHelper
                    {
                        Success = true,
                        Message = "¡Se actualizó con exito el periodo!"
                    };

                return new ResponseHelper
                {
                    Success = false,
                    Message = "¡Ups... no se actualizó el periodo... Ponte en Contacto con un Administrador!"
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
    }
}
