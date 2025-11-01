using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Core.Enums;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogosController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public CatalogosController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("generos")]
        public async Task<ActionResult<IEnumerable<Genero>>> GetGeneros()
        {
            var generos = await _dbContext.Genero.AsNoTracking().ToListAsync();

            return Ok(generos);
        }

        [HttpGet("horarios")]
        public async Task<ActionResult<IEnumerable<Horario>>> GetHorarios()
        {
            var horarios = await _dbContext.Turno.AsNoTracking().ToListAsync();

            return Ok(horarios);
        }

        [HttpGet("dias-semana")]
        public async Task<ActionResult<IEnumerable<DiaSemana>>> GetDiasSemana()
        {
            var diasSemana = await _dbContext.DiaSemana.AsNoTracking().ToListAsync();

            return Ok(diasSemana);
        }

        [HttpGet("estado-civil")]
        public async Task<ActionResult<IEnumerable<EstadoCivil>>> GetEstadoCivil()
        {
            var estadoCivil = await _dbContext.EstadoCivil.AsNoTracking().ToListAsync();

            return Ok(estadoCivil);
        }

        [HttpGet("aspirante-status")]
        public async Task<ActionResult<IEnumerable<AspiranteEstatus>>> GetAspiranteStatus()
        {
            var aspiranteEstatus = await _dbContext.AspiranteEstatus
                .Where(a => a.Status == StatusEnum.Active)
                .AsNoTracking()
                .ToListAsync();

            return Ok(aspiranteEstatus);
        }

        [HttpGet("medios-contacto")]
        public async Task<ActionResult<IEnumerable<MedioContacto>>> GetMediosContacto()
        {
            var mediosContacto = await _dbContext.MedioContacto
                .Where(mc => mc.Status == StatusEnum.Active)
                .AsNoTracking()
                .ToListAsync();

            return Ok(mediosContacto);
        }

        [HttpGet("user-roles")]
        public async Task<ActionResult<IEnumerable<string>>> GetRoles()
        {
            var roles = await _dbContext.Roles.AsNoTracking().Select(r => r.Name).ToListAsync();

            return Ok(roles);
        }

        [HttpGet("niveles-educativos")]
        public async Task<ActionResult<IEnumerable<NivelEducativo>>> GetNivelesEducativos()
        {
            var nivelesEducativos = await _dbContext.NivelEducativo
                .AsNoTracking()
                .Where(ne => ne.Activo)
                .ToListAsync();

            return Ok(nivelesEducativos);
        }

        [HttpGet("periodicidad")]
        public async Task<ActionResult<IEnumerable<Periodicidad>>> GetPeriodicidad()
        {
            var periodicidades = await _dbContext.Periodicidad.AsNoTracking().ToListAsync();

            return Ok(periodicidades);
        }
    }
}
