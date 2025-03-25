using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.ViewModels.Personas
{
    public class AlumnoPersonaVM
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int Matricula { get; set; }
        public string CursoEscolar { get; set; }
        public int IdPersona { get; set; }
        public int IdCursoEscolar { get; set; }
        public string NecesidadesEspeciales { get; set; }
        public string ContactoEmergencia { get; set; }
    }
}
