using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Core.Common;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Models;
using WebApplication2.Core.Requests.PeriodoAcademico;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodoAcademicoController : ControllerBase
    {
        private readonly IPeriodoAcademicoService _periodoAcademicoervice;
        private readonly IMapper _mapper;

        public PeriodoAcademicoController(IPeriodoAcademicoService periodoAcademicoService, IMapper mapper)
        {
            _periodoAcademicoervice = periodoAcademicoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<PeriodoAcademicoDto>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var pagination = await _periodoAcademicoervice.GetPeriodosAcademicos(page, pageSize);

            var periodosAcademicosDto = _mapper.Map<IEnumerable<PeriodoAcademicoDto>>(pagination.Items);

            var response = new PagedResult<PeriodoAcademicoDto>
            {
                TotalItems = pagination.TotalItems,
                Items = [.. periodosAcademicosDto],
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<PeriodoAcademicoDto>> Post([FromBody] PeriodoAcademicoRequest request)
        {
            var periodoAcademico = _mapper.Map<PeriodoAcademico>(request);

            await _periodoAcademicoervice.CrearPeriodoAcademico(periodoAcademico);

            var periodoAcademicoDto = _mapper.Map<PeriodoAcademicoDto>(periodoAcademico);

            return Ok(periodoAcademicoDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PeriodoAcademicoUpdateRequest request)
        {
            try
            {
                var newPeriodoAcademico = _mapper.Map<PeriodoAcademico>(request);

                var periodoAcademico = await _periodoAcademicoervice.ActualizarPeriodoAcademico(newPeriodoAcademico);

                var periodoAcademicoDto = _mapper.Map<PeriodoAcademicoDto>(periodoAcademico);

                return Ok(periodoAcademicoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
