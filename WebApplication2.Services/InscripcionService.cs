using Microsoft.EntityFrameworkCore;
using WebApplication2.Core.Common;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services
{
    public class InscripcionService : IInscripcionService
    {
        private readonly ApplicationDbContext _dbContext;

        public InscripcionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public async Task<PagedResult<Inscripcion>> GetInscripciones(int page, int pageSize)
        //{
        //    var totalItems = await _dbContext.Inscripcion
        //        .CountAsync();

        //    var inscripciones = await _dbContext.Inscripcion
        //        .Include(i => i.Estudiante)
        //        .Include(i => i.PlanEstudios)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();

        //    return new PagedResult<Inscripcion>
        //    {
        //        TotalItems = totalItems,
        //        Items = inscripciones,
        //        PageNumber = page,
        //        PageSize = pageSize
        //    };
        //}

        public async Task<Inscripcion> CrearInscripcion(Inscripcion inscripcion)
        {
            await _dbContext.AddAsync(inscripcion);
            await _dbContext.SaveChangesAsync();

            return inscripcion;
        }

        //public async Task<Inscripcion> EliminarInscripcion(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
