using WebApplication2.Core.Common;
using WebApplication2.Core.Models;

namespace WebApplication2.Services.Interfaces
{
    public interface IEstudianteService
    {
        Task<PagedResult<Estudiante>> GetEstudiantes(int page, int pageSize);
        Task<Estudiante> GetEstudianteDetalle(int id);
        Task<Estudiante> CrearEstudiante(Estudiante estudiante);
        Task<Estudiante> ActualizarEstudiante(Estudiante newEstudiante);
    }
}
