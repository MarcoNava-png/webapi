using Microsoft.EntityFrameworkCore;
using WebApplication2.Core.Common;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services
{
    public class DirectorService : IDirectorService
    {
        //private readonly ApplicationDbContext _dbContext;

        //public DirectorService(ApplicationDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public async Task<PagedResult<Director>> GetDirectores(int page, int pageSize)
        //{
        //    var totalItems = await _dbContext.Directores
        //        .Include(d => d.Persona)
        //        .Where(d => d.Persona.Estatus == StatusEnum.Activo)
        //        .CountAsync();

        //    var directores = await _dbContext.Directores
        //        .Include(d => d.Persona)
        //        .Include(d => d.Persona.PersonaGenero)
        //        .Where(d => d.Persona.Estatus == StatusEnum.Activo)
        //        .OrderBy(d => d.Persona.ApellidoPaterno)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();

        //    return new PagedResult<Director>
        //    {
        //        TotalItems = totalItems,
        //        Items = directores,
        //        PageNumber = page,
        //        PageSize = pageSize
        //    };
        //}

        //public async Task<Director> CrearDirector(Director director)
        //{
        //    await _dbContext.Directores.AddAsync(director);
        //    await _dbContext.SaveChangesAsync();

        //    return director;
        //}

        //public async Task<Director> EliminarDirector(int id)
        //{
        //    var director = await _dbContext.Directores
        //        .Include(d => d.Persona)
        //        .SingleOrDefaultAsync(p => p.Id == id);

        //    if (director == null)
        //    {
        //        throw new Exception("No existe persona con el id ingresado");
        //    }

        //    director.Persona.Estatus = StatusEnum.Inactivo;

        //    _dbContext.Directores.Update(director);

        //    await _dbContext.SaveChangesAsync();

        //    return director;
        //}
    }
}
