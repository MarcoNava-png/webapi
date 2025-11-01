using Microsoft.EntityFrameworkCore;
using WebApplication2.Core.Common;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services
{
    public class PeriodoAcademicoService : IPeriodoAcademicoService
    {
        private readonly ApplicationDbContext _dbContext;

        public PeriodoAcademicoService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<PeriodoAcademico>> GetPeriodosAcademicos(int page, int pageSize)
        {
            var totalItems = await _dbContext.PeriodoAcademico
                .AsNoTracking()
                .Where(p => p.Status == Core.Enums.StatusEnum.Active)
                .Include(p => p.IdPeriodicidadNavigation)
                .CountAsync();

            var periodosAcademicos = await _dbContext.PeriodoAcademico
                .AsNoTracking()
                .Where(p => p.Status == Core.Enums.StatusEnum.Active)
                .Include(p => p.IdPeriodicidadNavigation)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<PeriodoAcademico>
            {
                TotalItems = totalItems,
                Items = periodosAcademicos,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<PeriodoAcademico> CrearPeriodoAcademico(PeriodoAcademico periodoAcademico)
        {
            await _dbContext.PeriodoAcademico.AddAsync(periodoAcademico);
            await _dbContext.SaveChangesAsync();

            return periodoAcademico;
        }

        public async Task<PeriodoAcademico> ActualizarPeriodoAcademico(PeriodoAcademico newPeriodoAcademico)
        {
            var periodoAcademico = await _dbContext.PeriodoAcademico
                .FirstOrDefaultAsync(pe => pe.IdPeriodoAcademico == newPeriodoAcademico.IdPeriodoAcademico);

            if (periodoAcademico == null)
            {
                throw new Exception("No existe el periodo académico con el id ingresado");
            }

            periodoAcademico.Clave = newPeriodoAcademico.Clave;
            periodoAcademico.Nombre = newPeriodoAcademico.Nombre;
            periodoAcademico.IdPeriodicidad = newPeriodoAcademico.IdPeriodicidad;
            periodoAcademico.FechaInicio = newPeriodoAcademico.FechaInicio;
            periodoAcademico.FechaFin = newPeriodoAcademico.FechaFin;
            periodoAcademico.Status = newPeriodoAcademico.Status;

            _dbContext.PeriodoAcademico.Update(periodoAcademico);

            await _dbContext.SaveChangesAsync();

            return periodoAcademico;
        }
    }
}
