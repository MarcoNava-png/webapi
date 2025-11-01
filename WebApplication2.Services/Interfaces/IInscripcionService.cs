using WebApplication2.Core.Common;
using WebApplication2.Core.Models;

namespace WebApplication2.Services.Interfaces
{
    public interface IInscripcionService
    {
        //Task<PagedResult<Inscripcion>> GetInscripciones(int page, int pageSize);
        Task<Inscripcion> CrearInscripcion(Inscripcion inscripcion);
        //Task<Inscripcion> EliminarInscripcion(int id);
    }
}
