using Microsoft.EntityFrameworkCore;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services
{
    public class DepartamentoService : IDepartamentoService
    {
        //private readonly ApplicationDbContext _dbContext;

        //public DepartamentoService(ApplicationDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public async Task<IEnumerable<Departamento>> GetDepartamentos()
        //{
        //    return await _dbContext.Departamentos.ToListAsync();
        //}
    }
}
