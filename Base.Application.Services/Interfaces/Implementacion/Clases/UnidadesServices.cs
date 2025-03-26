using AutoMapper;
using Base.Application.Services.Interfaces.Contrato.Clases;
using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.ViewModels;
using Base.Infraestructura.Data.Repositorios.Contrato.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Base.Application.Services.Interfaces.Implementacion.Clases
{
    public class UnidadesServices : ServiceBase<UnidadesEntity, UnidadesEntityDTO>, IUnidadesServices
    {
        private readonly IUnidadesRepository _unidadesRepository;
        public UnidadesServices(IMapper mapper, IUnidadesRepository unidadesRepository) : base(mapper, unidadesRepository)
        {
            _unidadesRepository = unidadesRepository;
        }

        public async Task<ResponseHelper> GetUnidadesDeGrupo(int id)
        {
            try
            {
                List<UnidadesEntity> response = await _unidadesRepository.GetAllAsync(x => x.IdMateria == id && x.IdMateria == id);

                return new ResponseHelper
                {
                    Success = true,
                    Message = "Lista de Unidades del Grupo servida correctamente",
                    Data = response
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
