using Microsoft.EntityFrameworkCore;
using WebApplication2.Core.Common;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services
{
    public class CampusService : ICampusService
    {
        private readonly ApplicationDbContext _dbContext;

        public CampusService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<Campus>> GetCampuses(int page, int pageSize)
        {
            var totalItems = await _dbContext.Campus
                .Include(c => c.IdDireccionNavigation)
                .ThenInclude(dn => dn.CodigoPostal)
                .ThenInclude(cp => cp.Municipio)
                .ThenInclude(m => m.Estado)
                .Where(p => p.Status == Core.Enums.StatusEnum.Active)
                .CountAsync();

            var campuses = await _dbContext.Campus
                .Include(c => c.IdDireccionNavigation)
                .ThenInclude(dn => dn.CodigoPostal)
                .ThenInclude(cp => cp.Municipio)
                .ThenInclude(m => m.Estado)
                .Where(p => p.Status == Core.Enums.StatusEnum.Active)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Campus>
            {
                TotalItems = totalItems,
                Items = campuses,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<Campus> CrearCampus(Campus campus)
        {
            await _dbContext.AddAsync(campus);
            await _dbContext.SaveChangesAsync();

            return campus;
        }

        public async Task<Campus> ActualizarCampus(Campus newCampus)
        {
            var campus = await _dbContext.Campus
                .SingleOrDefaultAsync(p => p.IdCampus == newCampus.IdCampus);

            if (campus == null)
            {
                throw new Exception("No existe Campus con el id ingresado");
            }

            if (newCampus.IdDireccionNavigation != null)
            {
                var direccion = await _dbContext.Direccion.SingleOrDefaultAsync(d => d.IdDireccion == campus.IdDireccion);

                direccion.Calle = newCampus.IdDireccionNavigation.Calle;
                direccion.NumeroExterior = newCampus.IdDireccionNavigation.NumeroExterior;
                direccion.NumeroInterior = newCampus.IdDireccionNavigation.NumeroInterior;
                direccion.CodigoPostalId = newCampus.IdDireccionNavigation.CodigoPostalId;

                _dbContext.Direccion.Update(direccion);
            }

            campus.ClaveCampus = newCampus.ClaveCampus;

            _dbContext.Campus.Update(campus);

            await _dbContext.SaveChangesAsync();

            return campus;
        }
    }
}
