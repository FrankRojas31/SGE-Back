using AutoMapper;
using Base.Application.Service.Interfaces.Contracts.Seguridad;
using Base.Application.Services.Interfaces.Contrato.Clases;
using Base.Application.Services.Interfaces.Contrato.Escuela;
using Base.Application.Services.Interfaces.Contrato.Personas;
using Base.Application.Services.Interfaces.Implementacion.Clases;
using Base.Application.Services.Interfaces.Implementacion.Escuela;
using Base.Application.Services.Interfaces.Implementacion.Personas;
using Base.Application.Services.Interfaces.Implementacion.Seguridad;
using Base.Application.Services.Mapeo;
using Base.Infraestructura.Data.Repositorios.Contrato.Clases;
using Base.Infraestructura.Data.Repositorios.Contrato.Escuela;
using Base.Infraestructura.Data.Repositorios.Contrato.Personas;
using Base.Infraestructura.Data.Repositorios.Contrato.Seguridad;
using Base.Infraestructura.Data.Repositorios.Implementacion.Clases;
using Base.Infraestructura.Data.Repositorios.Implementacion.Escuela;
using Base.Infraestructura.Data.Repositorios.Implementacion.Personas;
using Base.Infraestructura.Data.Repositorios.Implementacion.Seguridad;
using Base.Infraestructura.Datos.ContextoBD;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Base.Application.Services.RegistroServicios
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            DotNetEnv.Env.Load();

            string connectionString = DotNetEnv.Env.GetString("DefaultConnection");

            configuration["ConnectionStrings:DefaultConnection"] = connectionString ?? configuration.GetConnectionString("DefaultConnection");

            #region DataBaseConnection
            services.AddDbContext<DataBaseContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            #endregion

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainToDtoMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            AddServices(services);
            AddRepository(services);


            return services;
        }

        private static void AddServices(IServiceCollection services)
        {
            // Servicios de Seguridad.
            services.AddScoped<IUserAccountService, UserAccountService>();

            // Servicios de Personas.
            services.AddScoped<IAlumnoServices, AlumnoServices>();
            services.AddScoped<IPersonaServices, PersonaServices>();

            // Servicios de Escuela.
            services.AddScoped<ICalificacionesServices, CalificacionesServices>();
            services.AddScoped<ICursoEscolarServices, CursoEscolarServices>();
            services.AddScoped<IPeriodosServices, PeriodosServices>();

            // Servicios de Clases.
            services.AddScoped<IGruposAlumnosServices, GruposAlumnosServices>();
            services.AddScoped<IGruposPeriodosServices, GruposPeriodosServices>();
            services.AddScoped<IMateriaGrupoServices, MateriaGrupoServices>();
            services.AddScoped<IGruposServices, GruposServices>();
            services.AddScoped<IMateriasServices, MateriasServices>();
            services.AddScoped<IUnidadesServices, UnidadesServices>();
        }

        private static void AddRepository(IServiceCollection services)
        {
            // Repositorios de Seguridad.
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            // Repositorios de Personas.
            services.AddScoped<IAlumnoRepository, AlumnoRepository>();
            services.AddScoped<IPersonaRepository, PersonaRepository>();

            // Repositorios de Escuela.
            services.AddScoped<ICalificacionesRepository, CalificacionesRepository>();
            services.AddScoped<ICursoEscolarRepository, CursoEscolarRepository>();
            services.AddScoped<IPeriodosRepository, PeriodosRepository>();

            // Repositorios de Clases.
            services.AddScoped<IGruposAlumnosRepository, GruposAlumnosRepository>();
            services.AddScoped<IGruposPeriodosRepository, GruposPeriodosRepository>();
            services.AddScoped<IMateriaGrupoRepository, MateriaGrupoRepository>();
            services.AddScoped<IGruposRepository, GruposRepository>();
            services.AddScoped<IMateriasRepository, MateriasRepository>();
            services.AddScoped<IUnidadesRepository, UnidadesRepository>();
        }
    }
}
