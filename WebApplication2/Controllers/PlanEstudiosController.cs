using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Core.Common;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Models;
using WebApplication2.Core.Requests.PlanEstudios;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanEstudiosController : ControllerBase
    {
        private readonly IPlanEstudioService _planEstudioService;
        private readonly IMapper _mapper;

        public PlanEstudiosController(IPlanEstudioService planEstudioService, IMapper mapper)
        {
            _planEstudioService = planEstudioService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<PlanEstudioDto>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var pagination = await _planEstudioService.GetPlanesEstudios(page, pageSize);

            var planesEstudiosDto = _mapper.Map<IEnumerable<PlanEstudioDto>>(pagination.Items);

            var response = new PagedResult<PlanEstudioDto>
            {
                TotalItems = pagination.TotalItems,
                Items = [.. planesEstudiosDto],
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<PlanEstudioDto>> Post([FromBody] PlanEstudiosRequest request)
        {
            var planEstudios = _mapper.Map<PlanEstudios>(request);

            await _planEstudioService.CrearPlanEstudios(planEstudios);

            var planEstudiosDto = _mapper.Map<PlanEstudioDto>(planEstudios);

            return Ok(planEstudiosDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PlanEstudiosUpdateRequest request)
        {
            try
            {
                var newPlanEstudios = _mapper.Map<PlanEstudios>(request);

                var planEstudios = await _planEstudioService.ActualizarPlanEstudios(newPlanEstudios);

                var planEstudiosDto = _mapper.Map<PlanEstudioDto>(planEstudios);

                return Ok(planEstudiosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
