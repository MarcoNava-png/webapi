using Microsoft.EntityFrameworkCore;
using WebApplication2.Configuration.Constants;
using WebApplication2.Core.Common;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services
{
    public class MateriaPlanService : IMateriaPlanService
    {
        private readonly ApplicationDbContext _dbContext;

        public MateriaPlanService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<MateriaPlan>> GetMateriaPlanes(int page, int pageSize)
        {
            var totalItems = await _dbContext.MateriaPlan
                .Include(mp => mp.IdMateriaNavigation)
                .Include(mp => mp.IdPlanEstudiosNavigation)
                .Where(d => d.Status == Core.Enums.StatusEnum.Active)
                .CountAsync();

            var items = await _dbContext.MateriaPlan
                .Include(mp => mp.IdMateriaNavigation)
                .Include(mp => mp.IdPlanEstudiosNavigation)
                .Where(d => d.Status == Core.Enums.StatusEnum.Active)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<MateriaPlan>
            {
                TotalItems = totalItems,
                Items = items,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<MateriaPlan> GetMateriaPlanDetalle(int id)
        {
            var materiaPlan = await _dbContext.MateriaPlan
                .Include(mp => mp.IdMateriaNavigation)
                .Include(mp => mp.IdPlanEstudiosNavigation)
                .Where(d => d.Status == Core.Enums.StatusEnum.Active)
                .FirstOrDefaultAsync(e => e.IdMateriaPlan == id && e.Status == Core.Enums.StatusEnum.Active);

            if (materiaPlan == null)
            {
                throw new Exception(ErrorConstants.RECORD_NOTFOUND);
            }

            return materiaPlan;
        }

        public async Task<MateriaPlan> CrearMateriaPlan(MateriaPlan materiaPlan)
        {
            await _dbContext.MateriaPlan.AddAsync(materiaPlan);
            await _dbContext.SaveChangesAsync();

            return materiaPlan;
        }

        public async Task<MateriaPlan> ActualizarMateriaPlan(MateriaPlan newMateriaPlan)
        {
            var materiaPlan = await _dbContext.MateriaPlan
                .FirstOrDefaultAsync(e => e.IdMateriaPlan == newMateriaPlan.IdMateriaPlan);

            if (materiaPlan == null)
            {
                throw new Exception(ErrorConstants.RECORD_NOTFOUND);
            }

            materiaPlan.IdPlanEstudios = newMateriaPlan.IdPlanEstudios;
            materiaPlan.IdMateria = newMateriaPlan.IdMateria;
            materiaPlan.Cuatrimestre = newMateriaPlan.Cuatrimestre;
            materiaPlan.EsOptativa = newMateriaPlan.EsOptativa;
            materiaPlan.Status = newMateriaPlan.Status;

            _dbContext.MateriaPlan.Update(materiaPlan);

            await _dbContext.SaveChangesAsync();

            return materiaPlan;
        }
    }
}