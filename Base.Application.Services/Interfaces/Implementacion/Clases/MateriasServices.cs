using AutoMapper;
using Base.Application.Services.Interfaces.Contrato.Clases;
using Base.Domain.DTOs.Clases;
using Base.Domain.Entidades.Clases;
using Base.Domain.ViewModels;
using Base.Infraestructura.Data.Repositorios.Contrato;
using Base.Infraestructura.Data.Repositorios.Contrato.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Services.Interfaces.Implementacion.Clases
{
    public class MateriasServices : ServiceBase<MateriasEntity, MateriasEntityDTO>, IMateriasServices
    {
        private readonly IMateriasRepository _materiasRepository;
        public MateriasServices(IMapper mapper, IMateriasRepository materiasRepository) : base(mapper, materiasRepository)
        {
            _materiasRepository = materiasRepository;
        }

        public async Task<ResponseHelper> GetUnidadesDeMateria(int id)
        {
            try
            {
                MateriasEntity materia = await _materiasRepository.GetSingleAsync(x => x.Id == id && x.EsBorrado == false);
                
                if(materia is not null)
                {
                    List<UnidadesEntity> unidadesDeMateria = await _materiasRepository.GetUnidadesDeGrupo(id);

                    return new ResponseHelper
                    {
                        Success = true,
                        Message = "¡Lista de Unidades del Grupo servida correctamente!",
                        Data = unidadesDeMateria,
                    };
                } else
                {
                    return new ResponseHelper
                    {
                        Success = false,
                        Message = "¡Cuidado esta materia no existe!"
                    };
                }
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
