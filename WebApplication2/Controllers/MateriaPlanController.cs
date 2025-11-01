using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Core.Common;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Models;
using WebApplication2.Core.Requests.NewFolder;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriaPlanController : ControllerBase
    {
        private readonly IMateriaPlanService _materiaPlanService;
        private readonly IMapper _mapper;

        public MateriaPlanController(IMateriaPlanService materiaPlanService, IMapper mapper)
        {
            _materiaPlanService = materiaPlanService;
            _mapper = mapper;
        }

        // GET: api/MateriaPlan
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var pagination = await _materiaPlanService.GetMateriaPlanes(page, pageSize);

            var estudiantesDto = _mapper.Map<IEnumerable<MateriaPlanDto>>(pagination.Items);

            var response = new PagedResult<MateriaPlanDto>
            {
                TotalItems = pagination.TotalItems,
                Items = [.. estudiantesDto],
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };

            return Ok(response);
        }

        // GET: api/MateriaPlan/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var materiaPlan = await _materiaPlanService.GetMateriaPlanDetalle(id);

            if (materiaPlan == null)
            {
                return NotFound();
            }

            return Ok(materiaPlan);
        }

        // POST: api/MateriaPlan
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MateriaPlanRequest request)
        {
            var newMateriaPlan = _mapper.Map<MateriaPlan>(request);

            var materiaPlan = await _materiaPlanService.CrearMateriaPlan(newMateriaPlan);

            var materiaPlanDto = _mapper.Map<MateriaPlanDto>(materiaPlan);

            return CreatedAtAction(nameof(GetById), new { id = materiaPlanDto.IdMateriaPlan }, materiaPlanDto);
        }

        // PUT: api/MateriaPlan/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MateriaPlanUpdateRequest request)
        {
            var newMateriaPlan = _mapper.Map<MateriaPlan>(request);

            try
            {
                var materiaPlan = await _materiaPlanService.ActualizarMateriaPlan(newMateriaPlan);

                var materiaPlanDto = _mapper.Map<MateriaPlanDto>(materiaPlan);

                return Ok(materiaPlanDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}