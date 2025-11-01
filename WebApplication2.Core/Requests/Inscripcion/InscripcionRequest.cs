namespace WebApplication2.Core.Requests.Inscripcion
{
    public class InscripcionRequest
    {
        public int IdEstudiante { get; set; }

        public int IdGrupoMateria { get; set; }

        public DateTime FechaInscripcion { get; set; }

        public string Estado { get; set; } = null!;
    }
}
