using WebApplication2.Core.Common;
using WebApplication2.Core.Models;

namespace WebApplication2.Services.Interfaces
{
    public interface IProfesorService
    {
        Task<PagedResult<Profesor>> GetProfesores(int campusId, int page, int pageSize);
        Task<Profesor> CrearProfesor(Profesor profesor);
        Task<Profesor> ActualizarProfesor(Profesor newProfesor);
    }
}
