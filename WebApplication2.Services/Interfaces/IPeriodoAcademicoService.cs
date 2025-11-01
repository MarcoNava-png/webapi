using WebApplication2.Core.Common;
using WebApplication2.Core.Models;

namespace WebApplication2.Services.Interfaces
{
    public interface IPeriodoAcademicoService
    {
        Task<PagedResult<PeriodoAcademico>> GetPeriodosAcademicos(int page, int pageSize);
        Task<PeriodoAcademico> CrearPeriodoAcademico(PeriodoAcademico periodoAcademico);
        Task<PeriodoAcademico> ActualizarPeriodoAcademico(PeriodoAcademico newPeriodoAcademico);
    }
}
