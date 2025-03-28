using AutoMapper;
using Base.Domain.DTO.Core;
using Base.Domain.DTOs.Clases;
using Base.Domain.DTOs.Core;
using Base.Domain.DTOs.Escuela;
using Base.Domain.DTOs.Personas;
using Base.Domain.Entidades.Clases;
using Base.Domain.Entidades.Core;
using Base.Domain.Entidades.Escuela;
using Base.Domain.Entidades.Personas;

namespace Base.Application.Services.Mapeo
{
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            this.CreateMap<BaseEntity, BaseDTO>();

            // Mapeo de Personas.
            this.CreateMap<AlumnoEntity, AlumnoEntityDTO>().ReverseMap();
            this.CreateMap<PersonaEntity, PersonaEntityDTO>().ReverseMap();

            // Mapeo de Escuela.
            this.CreateMap<CursoEscolarEntity, CursoEscolarEntityDTO>().ReverseMap();
            this.CreateMap<CalificacionesEntity, CalificacionesEntityDTO>().ReverseMap();
            this.CreateMap<PeriodosEntity, PeriodosEntityDTO>().ReverseMap();

            // Mapeo de Clases.
            this.CreateMap<GruposAlumnosEntity, GruposAlumnosEntityDTO>().ReverseMap();
            this.CreateMap<GruposEntity, GruposEntityDTO>().ReverseMap();
            this.CreateMap<GruposPeriodosEntity, GruposPeriodosEntityDTO>().ReverseMap();
            this.CreateMap<MateriaGrupoEntity, MateriaGrupoEntityDTO>().ReverseMap();
            this.CreateMap<MateriasEntity, MateriasEntityDTO>().ReverseMap();
            this.CreateMap<UnidadesEntity, UnidadesEntityDTO>().ReverseMap();

            // Mapeo de Errores.
            this.CreateMap<ErrorLogs, ErrorLogsDTO>().ReverseMap();
        }
    }
}
