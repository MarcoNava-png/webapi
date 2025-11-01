using Microsoft.EntityFrameworkCore;
using WebApplication2.Core.Common;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services
{
    public class PlanEstudiosService : IPlanEstudioService
    {
        private readonly ApplicationDbContext _dbContext;

        public PlanEstudiosService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<PlanEstudios>> GetPlanesEstudios(int page, int pageSize)
        {
            var totalItems = await _dbContext.PlanEstudios
                .Include(p => p.IdCampusNavigation)
                .Include(p => p.IdPeriodicidadNavigation)
                .CountAsync();

            var profesores = await _dbContext.PlanEstudios
                .Include(p => p.IdCampusNavigation)
                .Include(p => p.IdPeriodicidadNavigation)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResult<PlanEstudios>
            {
                TotalItems = totalItems,
                Items = profesores,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<PlanEstudios> CrearPlanEstudios(PlanEstudios planEstudios)
        {
            await _dbContext.PlanEstudios.AddAsync(planEstudios);
            await _dbContext.SaveChangesAsync();

            return planEstudios;
        }

        public async Task<PlanEstudios> ActualizarPlanEstudios(PlanEstudios newPlanEstudios)
        {
            var planEstudios = await _dbContext.PlanEstudios
                .FirstOrDefaultAsync(pe => pe.IdPlanEstudios == newPlanEstudios.IdPlanEstudios);

            if (planEstudios == null)
            {
                throw new Exception("No existe el plan de estudios con el id ingresado");
            }

            planEstudios.ClavePlanEstudios = newPlanEstudios.ClavePlanEstudios;
            planEstudios.NombrePlanEstudios = newPlanEstudios.NombrePlanEstudios;
            planEstudios.RVOE = newPlanEstudios.RVOE;
            planEstudios.PermiteAdelantar = newPlanEstudios.PermiteAdelantar;
            planEstudios.Version = newPlanEstudios.Version;
            planEstudios.DuracionMeses = newPlanEstudios.DuracionMeses;
            planEstudios.MinimaAprobatoriaParcial = newPlanEstudios.MinimaAprobatoriaParcial;
            planEstudios.MinimaAprobatoriaFinal = newPlanEstudios.MinimaAprobatoriaFinal;
            planEstudios.IdPeriodicidad = newPlanEstudios.IdPeriodicidad;
            planEstudios.IdNivelEducativo = newPlanEstudios.IdNivelEducativo;
            planEstudios.IdCampus = newPlanEstudios.IdCampus;
            planEstudios.Status = newPlanEstudios.Status;

            _dbContext.PlanEstudios.Update(planEstudios);

            await _dbContext.SaveChangesAsync();

            return planEstudios;
        }
    }
}
