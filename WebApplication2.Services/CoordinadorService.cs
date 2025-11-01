using Microsoft.EntityFrameworkCore;
using WebApplication2.Core.Common;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services
{
    public class CoordinadorService : ICoordinadorService
    {
        //private readonly ApplicationDbContext _dbContext;

        //public CoordinadorService(ApplicationDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public async Task<PagedResult<Coordinador>> GetCoordinadores(int page, int pageSize)
        //{
        //    var totalItems = await _dbContext.Coordinadores
        //        .Where(d => d.Persona.Estatus == StatusEnum.Activo)
        //        .CountAsync();

        //    var coordinadores = await _dbContext.Coordinadores
        //        .Include(d => d.Persona)
        //        .Include(d => d.Persona.PersonaGenero)
        //        .Where(d => d.Persona.Estatus == StatusEnum.Activo)
        //        .OrderBy(d => d.Persona.ApellidoPaterno)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();

        //    return new PagedResult<Coordinador>
        //    {
        //        TotalItems = totalItems,
        //        Items = coordinadores,
        //        PageNumber = page,
        //        PageSize = pageSize
        //    };
        //}

        //public async Task<Coordinador> CrearCoordinador(Coordinador coordinador)
        //{
        //    await _dbContext.Coordinadores.AddAsync(coordinador);
        //    await _dbContext.SaveChangesAsync();

        //    return coordinador;
        //}

        //public async Task<Coordinador> EliminarCoordinador(int id)
        //{
        //    var coordinador = await _dbContext.Coordinadores
        //        .Include(d => d.Persona)
        //        .SingleOrDefaultAsync(p => p.Id == id);

        //    if (coordinador == null)
        //    {
        //        throw new Exception("No existe persona con el id ingresado");
        //    }

        //    coordinador.Persona.Estatus = StatusEnum.Inactivo;

        //    _dbContext.Coordinadores.Update(coordinador);

        //    await _dbContext.SaveChangesAsync();

        //    return coordinador;
        //}
    }
}
