using WebApplication2.Core.Common;
using WebApplication2.Core.Models;

namespace WebApplication2.Services.Interfaces
{
    public interface ICampusService
    {
        Task<PagedResult<Campus>> GetCampuses(int page, int pageSize);
        Task<Campus> CrearCampus(Campus campus);
        Task<Campus> ActualizarCampus(Campus newCampus);
    }
}
