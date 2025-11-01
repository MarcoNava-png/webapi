using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Configuration.Constants;
using WebApplication2.Core.Common;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Models;
using WebApplication2.Core.Requests.Profesor;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorController : ControllerBase
    {
        private readonly IProfesorService _profesorService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public ProfesorController(IProfesorService profesorService, IAuthService authService, IMapper mapper)
        {
            _profesorService = profesorService;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpGet("{campusId}")]
        public async Task<ActionResult<PagedResult<ProfesorDto>>> Get(int campusId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var pagination = await _profesorService.GetProfesores(campusId, page, pageSize);

            var profesoresDto = _mapper.Map<IEnumerable<ProfesorDto>>(pagination.Items);

            var response = new PagedResult<ProfesorDto>
            {
                TotalItems = pagination.TotalItems,
                Items = [.. profesoresDto],
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ProfesorDto>> Profesor([FromBody] ProfesorRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.EmailInstitucional,
                Email = request.EmailInstitucional,
            };

            try
            {
                var signupResponse = await _authService.Signup(user, request.NoEmpleado, [Rol.DOCENTE]);

                var newProfesor = _mapper.Map<Profesor>(request);

                var profesor = await _profesorService.CrearProfesor(newProfesor);

                var profesorDto = _mapper.Map<ProfesorDto>(profesor);

                return Ok(profesorDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProfesorUpdateRequest request)
        {
            try
            {
                var newProfesor = _mapper.Map<Profesor>(request);

                await _profesorService.ActualizarProfesor(newProfesor);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
