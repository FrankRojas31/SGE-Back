using Base.Domain.Entidades.Clases;
using Base.Domain.Entidades.Escuela;
using Base.Domain.Entidades.Personas;
using Base.Domain.Entidades.Seguridad;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Base.Infraestructura.Datos.ContextoBD
{
    public class DataBaseContext(DbContextOptions<DataBaseContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        DbSet<GruposAlumnosEntity> GruposAlumnos { get; set; }
        DbSet<GruposEntity> Grupos { get; set; }
        DbSet<GruposPeriodosEntity> GruposPeriodos { get; set; }
        DbSet<MateriasEntity> Materias { get; set; }
        DbSet<UnidadesEntity> Unidades { get; set; }
        DbSet<CalificacionesEntity> Calificaciones { get; set; }
        DbSet<CursoEscolarEntity> CursoEscolar { get; set; }
        DbSet<PeriodosEntity> Periodos { get; set; }
        DbSet<AlumnoEntity> Alumno { get; set; }
        DbSet<PersonaEntity> Persona { get; set; }
    }
}
