using Microsoft.EntityFrameworkCore;
using WebApplication2.Core.Common;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly ApplicationDbContext _dbContext;

        public EstudianteService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<Estudiante>> GetEstudiantes(int page, int pageSize)
        {
            var totalItems = await _dbContext.Estudiante
                .Include(d => d.IdPersonaNavigation)
                .Where(d => d.Status == Core.Enums.StatusEnum.Active)
                .CountAsync();

            var items = await _dbContext.Estudiante
                .Include(d => d.IdPersonaNavigation)
                .Where(d => d.Status == Core.Enums.StatusEnum.Active)
                .OrderBy(d => d.IdPersonaNavigation.ApellidoPaterno)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Estudiante>
            {
                TotalItems = totalItems,
                Items = items,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<Estudiante> GetEstudianteDetalle(int id)
        {
            var totalItems = await _dbContext.Estudiante
                .Where(d => d.Status == Core.Enums.StatusEnum.Active)
                .CountAsync();

            var estudiante = await _dbContext.Estudiante
                .Include(d => d.IdPersonaNavigation)
                .Include(d => d.Inscripcion)
                .ThenInclude(i => i.IdGrupoMateriaNavigation)
                .ThenInclude(gm => gm.IdMateriaPlanNavigation)
                .ThenInclude(mp => mp.IdMateriaNavigation)
                .FirstOrDefaultAsync(e => e.IdEstudiante == id && e.Status == Core.Enums.StatusEnum.Active);

            if (estudiante == null)
            {
                throw new Exception("No se ha encontrado estudiante con el id ingreado.");
            }

            return estudiante;
        }

        public async Task<Estudiante> CrearEstudiante(Estudiante estudiante)
        {
            await _dbContext.Estudiante.AddAsync(estudiante);
            await _dbContext.SaveChangesAsync();

            return estudiante;
        }

        public async Task<Estudiante> ActualizarEstudiante(Estudiante newEstudiante)
        {
            var estudiante = await _dbContext.Estudiante
                .FirstOrDefaultAsync(e => e.IdEstudiante == newEstudiante.IdEstudiante);

            if (estudiante == null)
            {
                throw new Exception("No existe estudiante con el id ingresado");
            }

            estudiante.Matricula = newEstudiante.Matricula;
            estudiante.UsuarioId = newEstudiante.UsuarioId;
            estudiante.Email = newEstudiante.Email;

            _dbContext.Estudiante.Update(estudiante);

            await _dbContext.SaveChangesAsync();

            return estudiante;
        }
    }
}
