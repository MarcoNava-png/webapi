using WebApplication2.Core.Common;
using WebApplication2.Core.Models;

namespace WebApplication2.Services.Interfaces
{
    public interface IPlanEstudioService
    {
        Task<PagedResult<PlanEstudios>> GetPlanesEstudios(int page, int pageSize);
        Task<PlanEstudios> CrearPlanEstudios(PlanEstudios planEstudios);
        Task<PlanEstudios> ActualizarPlanEstudios(PlanEstudios newPlanEstudios);
    }
}
