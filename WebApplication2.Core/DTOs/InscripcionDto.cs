using WebApplication2.Core.Models;

namespace WebApplication2.Core.DTOs
{
    public class InscripcionDto
    {
        public int IdInscripcion { get; set; }

        public int IdEstudiante { get; set; }

        public int IdGrupoMateria { get; set; }

        public DateTime FechaInscripcion { get; set; }

        public string Estado { get; set; } = null!;

        public decimal? CalificacionFinal { get; set; }

        public virtual Estudiante IdEstudianteNavigation { get; set; } = null!;

        public virtual GrupoMateria IdGrupoMateriaNavigation { get; set; } = null!;
    }
}
