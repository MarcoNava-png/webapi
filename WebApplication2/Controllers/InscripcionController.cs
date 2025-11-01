using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Models;
using WebApplication2.Core.Requests.Inscripcion;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [Route("api/inscripciones")]
    [ApiController]
    public class InscripcionController : ControllerBase
    {
        private readonly IInscripcionService _inscripcionService;
        private readonly IMapper _mapper;

        public InscripcionController(IInscripcionService inscripcionService, IMapper mapper)
        {
            _inscripcionService = inscripcionService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<InscripcionDto>> Inscripcion([FromBody] InscripcionRequest request)
        {
            try
            {
                var newInscripcion = new Inscripcion
                {
                    IdEstudiante = request.IdEstudiante,
                    IdGrupoMateria = request.IdGrupoMateria,
                    Estado = "Inscrito"
                };

                await _inscripcionService.CrearInscripcion(newInscripcion);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
