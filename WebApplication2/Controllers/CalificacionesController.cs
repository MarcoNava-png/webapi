using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Core.Common;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Enums;
using WebApplication2.Core.Models;
using WebApplication2.Core.Requests.Aspirante;
using WebApplication2.Core.Requests.PlanEstudios;
using WebApplication2.Services;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionesController : ControllerBase
    {
        private readonly ICalificacionesService _calificacionesService;
        private readonly IMapper _mapper;

        public CalificacionesController(ICalificacionesService CalificacionesService, IMapper mapper)
        {
            _calificacionesService = CalificacionesService;
            _mapper = mapper;
        }

        [HttpGet("{grupoMateriaId}/{parcialId}")]
        public async Task<ActionResult<IEnumerable<CalificacionParcialResponse>>> Get(int grupoMateriaId, int parcialId)
        {
            var parcialesGrupo = await _calificacionesService.GetParcialesPorGrupo(grupoMateriaId, parcialId);

            var calificacionParcialResponse = _mapper.Map<IEnumerable<CalificacionParcialResponse>>(parcialesGrupo);

            return Ok(calificacionParcialResponse);
        }

        
        [HttpPost("parciales")]
        public async Task<ActionResult<CalificacionParcialResponse>> CrearParcial([FromBody] CalificacionParcialCreateRequest req)
        {
            var acta = _mapper.Map<CalificacionParcial>(req);
            acta.StatusParcial = StatusParcialEnum.Abierto;
            acta.FechaApertura = req.FechaApertura ?? DateTime.UtcNow;

            var creado = await _calificacionesService.AbrirParcial(acta);

            // Mapea Nombres desde tus relaciones (GrupoMateria/Materia/Profesor)
            var dto = _mapper.Map<CalificacionParcialResponse>(creado);
            return CreatedAtAction(nameof(GetParcialById), new { id = creado.Id }, dto);
        }

        [HttpGet("parciales/{id:int}")]
        public async Task<ActionResult<CalificacionParcialResponse>> GetParcialById(int id)
        {
            var acta = await _calificacionesService.GetParcialById(id);
            if (acta is null) return NotFound();
            return _mapper.Map<CalificacionParcialResponse>(acta);
        }

        [HttpPatch("parciales/{id:int}/estado")]
        public async Task<IActionResult> CambiarEstadoParcial(int id, [FromBody] CalificacionParcialEstadoRequest req)
        {
            // si usas enum:
            await _calificacionesService.CambiarEstadoParcial(id, req.StatusParcial.ToString(), User?.Identity?.Name ?? "sistema");
            return NoContent();
        }

        [HttpPost("detalle")]
        public async Task<ActionResult<CalificacionDetalleItemResponse>> UpsertDetalle([FromBody] CalificacionDetalleUpsertRequest req)
        {
            var entity = _mapper.Map<CalificacionDetalle>(req);
            var username = User?.Identity?.Name ?? "sistema";

            await _calificacionesService.UpsertDetalle(entity, username);

            var dto = _mapper.Map<CalificacionDetalleItemResponse>(entity);
            return Ok(dto);
        }
    }
}
