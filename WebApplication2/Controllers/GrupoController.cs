using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Core.Common;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Models;
using WebApplication2.Core.Requests.Grupo;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [Route("api/grupos")]
    [ApiController]
    public class GrupoController : ControllerBase
    {
        private readonly IGrupoService _grupoService;
        private readonly IMapper _mapper;

        public GrupoController(IGrupoService grupoService, IMapper mapper)
        {
            _grupoService = grupoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<GrupoDto>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var pagination = await _grupoService.GetGrupos(page, pageSize);

            var gruposDto = _mapper.Map<IEnumerable<GrupoDto>>(pagination.Items);

            var response = new PagedResult<GrupoDto>
            {
                TotalItems = pagination.TotalItems,
                Items = [.. gruposDto],
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };

            return Ok(response);
        }

        [HttpGet("{idGrupo}")]
        public async Task<ActionResult<GrupoDetalleDto>> GetDetalle(int idGrupo)
        {
            var grupoDetalle = await _grupoService.GetDetalleGrupo(idGrupo);

            var gruposDetaleDto = _mapper.Map<GrupoDetalleDto>(grupoDetalle);

            return Ok(gruposDetaleDto);
        }

        [HttpPost]
        public async Task<ActionResult<GrupoDto>> Grupo([FromBody] GrupoRequest request)
        {
            try
            {
                var newGrupo = _mapper.Map<Grupo>(request);

                await _grupoService.CrearGrupo(newGrupo);

                var grupoDto = _mapper.Map<GrupoDto>(newGrupo);

                return Ok(grupoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("carga-materias")]
        public async Task<ActionResult> CargaMateria([FromBody] CargaGrupoMateriasRequest request)
        {
            try
            {
                var grupoMaterias = _mapper.Map<List<GrupoMateria>>(request.GrupoMaterias);

                grupoMaterias.ForEach(gm => gm.IdGrupo = request.IdGrupo);

                await _grupoService.CargarMateriasGrupo(grupoMaterias);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] GrupoUpdateRequest request)
        {
            try
            {
                var newGrupo = _mapper.Map<Grupo>(request);

                await _grupoService.ActualizarGrupo(newGrupo);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
