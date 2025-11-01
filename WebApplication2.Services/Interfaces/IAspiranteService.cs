using WebApplication2.Core.Common;
using WebApplication2.Core.Models;

namespace WebApplication2.Services.Interfaces
{
    public interface IAspiranteService
    {
        Task<PagedResult<Aspirante>> GetAspirantes(int page, int pageSize, string filter);
        Task<Aspirante> GetAspiranteByPersonaId(int id);
        Task<Aspirante> CrearAspirante(Aspirante aspirante);
        Task<Aspirante> ActualizarAspirante(Aspirante aspirante);
        Task<IEnumerable<AspiranteBitacoraSeguimiento>> GetBitacoraSeguimiento(int aspiranteId);
        Task<AspiranteBitacoraSeguimiento> CrearSeguimiento(AspiranteBitacoraSeguimiento seguimiento);
        Task<Aspirante> GetAspiranteById(int id);
    }
}
