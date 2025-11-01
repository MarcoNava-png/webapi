using Microsoft.EntityFrameworkCore;
using WebApplication2.Core.Common;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services
{
    public class GrupoService: IGrupoService
    {
        private readonly ApplicationDbContext _dbContext;

        public GrupoService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<Grupo>> GetGrupos(int page, int pageSize)
        {
            var totalItems = await _dbContext.Grupo
                .Include(g => g.IdPeriodoAcademicoNavigation)
                .Include(g => g.IdTurnoNavigation)
                .Include(g => g.IdPlanEstudiosNavigation)
                .Where(d => d.Status == Core.Enums.StatusEnum.Active)
                .CountAsync();

            var items = await _dbContext.Grupo
                .Include(g => g.IdPeriodoAcademicoNavigation)
                .Include(g => g.IdTurnoNavigation)
                .Include(g => g.IdPlanEstudiosNavigation)
                .Where(d => d.Status == Core.Enums.StatusEnum.Active)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Grupo>
            {
                TotalItems = totalItems,
                Items = items,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<Grupo> GetDetalleGrupo(int idGrupo)
        {
            var grupo = await _dbContext.Grupo
                .Include(g => g.GrupoMateria)
                .ThenInclude(gm => gm.IdProfesorNavigation)
                .ThenInclude(pn => pn.IdPersonaNavigation)
                .Include(g => g.GrupoMateria)
                .ThenInclude(gm => gm.IdMateriaPlanNavigation)
                .ThenInclude(mpn => mpn.IdMateriaNavigation)
                .Include(g => g.IdPeriodoAcademicoNavigation)
                .Include(g => g.IdPlanEstudiosNavigation)
                .Include(g => g.IdTurnoNavigation)
                .FirstOrDefaultAsync(g => g.IdGrupo == idGrupo);

            if (grupo  == null)
            {
                throw new Exception("No existe grupo con el id ingresado");
            }

            return grupo;
        }

        public async Task<Grupo> CrearGrupo(Grupo grupo)
        {
            await _dbContext.Grupo.AddAsync(grupo);
            await _dbContext.SaveChangesAsync();

            return grupo;
        }

        public async Task<IEnumerable<GrupoMateria>> CargarMateriasGrupo(IEnumerable<GrupoMateria> grupoMaterias)
        {
            await _dbContext.GrupoMateria.AddRangeAsync(grupoMaterias);
            await _dbContext.SaveChangesAsync();

            return grupoMaterias;
        }

        public async Task<Grupo> ActualizarGrupo(Grupo newGrupo)
        {
            var item = await _dbContext.Grupo
                .SingleOrDefaultAsync(g => g.IdGrupo == newGrupo.IdGrupo);

            if (item == null)
            {
                throw new Exception("No existe grupo con el id ingresado");
            }

            item.IdPlanEstudios = newGrupo.IdPlanEstudios;
            item.IdPeriodoAcademico = newGrupo.IdPeriodoAcademico;
            item.NumeroCuatrimestre = newGrupo.NumeroCuatrimestre;
            item.NumeroGrupo = newGrupo.NumeroGrupo;
            item.IdTurno = newGrupo.IdTurno;
            item.CapacidadMaxima = newGrupo.CapacidadMaxima;
            item.Status = newGrupo.Status;

            _dbContext.Grupo.Update(item);

            await _dbContext.SaveChangesAsync();

            return item;
        }
    }
}
