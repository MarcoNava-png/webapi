using Microsoft.EntityFrameworkCore;
using WebApplication2.Core.Common;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services
{
    public class ProfesorService : IProfesorService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProfesorService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<Profesor>> GetProfesores(int campusId, int page, int pageSize)
        {
            var totalItems = await _dbContext.Profesor
                .Include(d => d.IdPersonaNavigation)
                .ThenInclude(p => p.IdGeneroNavigation)
                .Where(p => p.Status == Core.Enums.StatusEnum.Active && p.CampusId == campusId)
                .CountAsync();

            var profesores = await _dbContext.Profesor
                .Include(d => d.IdPersonaNavigation)
                .ThenInclude(p => p.IdGeneroNavigation)
                .Where(p => p.Status == Core.Enums.StatusEnum.Active && p.CampusId == campusId)
                .OrderBy(p => p.IdPersonaNavigation.ApellidoPaterno)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Profesor>
            {
                TotalItems = totalItems,
                Items = profesores,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<Profesor> CrearProfesor(Profesor profesor)
        {
            await _dbContext.AddAsync(profesor);
            await _dbContext.SaveChangesAsync();

            return profesor;
        }

        public async Task<Profesor> ActualizarProfesor(Profesor newProfesor)
        {
            var profesor = await _dbContext.Profesor
                .Include(d => d.IdPersonaNavigation)
                .SingleOrDefaultAsync(p => p.IdProfesor == newProfesor.IdProfesor);

            if (profesor == null)
            {
                throw new Exception("No existe profesor con el id ingresado");
            }

            if (newProfesor.IdPersonaNavigation != null)
            {
                var persona = await _dbContext.Persona.SingleOrDefaultAsync(p => p.IdPersona == profesor.IdPersona);

                persona.Nombre = newProfesor.IdPersonaNavigation.Nombre;
                persona.ApellidoPaterno = newProfesor.IdPersonaNavigation.ApellidoPaterno;
                persona.ApellidoMaterno = newProfesor.IdPersonaNavigation.ApellidoMaterno;
                persona.FechaNacimiento = newProfesor.IdPersonaNavigation.FechaNacimiento;
                persona.IdGenero = newProfesor.IdPersonaNavigation.IdGenero;
                persona.Curp = newProfesor.IdPersonaNavigation.Curp;
                persona.Correo = newProfesor.IdPersonaNavigation.Correo;
                persona.Telefono = newProfesor.IdPersonaNavigation.Telefono;

                _dbContext.Persona.Update(persona);
            }

            if (newProfesor.IdPersonaNavigation != null && newProfesor.IdPersonaNavigation.IdDireccionNavigation != null)
            {
                var direccion = await _dbContext.Direccion.SingleOrDefaultAsync(d => d.IdDireccion == profesor.IdPersonaNavigation.IdDireccion);

                direccion.Calle = newProfesor.IdPersonaNavigation.IdDireccionNavigation.Calle;
                direccion.NumeroExterior = newProfesor.IdPersonaNavigation.IdDireccionNavigation.NumeroExterior;
                direccion.NumeroInterior = newProfesor.IdPersonaNavigation.IdDireccionNavigation.NumeroInterior;
                direccion.CodigoPostalId = newProfesor.IdPersonaNavigation.IdDireccionNavigation.CodigoPostalId;

                _dbContext.Direccion.Update(direccion);
            }

            profesor.NoEmpleado = newProfesor.NoEmpleado;
            profesor.EmailInstitucional = newProfesor.EmailInstitucional;
            profesor.Status = newProfesor.Status;

            _dbContext.Profesor.Update(profesor);

            await _dbContext.SaveChangesAsync();

            return profesor;
        }
    }
}
