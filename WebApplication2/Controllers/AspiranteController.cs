using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Core.Common;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Models;
using WebApplication2.Core.Requests.Aspirante;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AspiranteController : ControllerBase
    {
        private readonly IAspiranteService _aspiranteService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AspiranteController(IAspiranteService aspiranteService, IMapper mapper, IAuthService authService)
        {
            _aspiranteService = aspiranteService;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<AspiranteDto>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string filter = "")
        {
            var pagination = await _aspiranteService.GetAspirantes(page, pageSize, filter);

            var aspirantesDtos = _mapper.Map<IEnumerable<AspiranteDto>>(pagination.Items);

            foreach (var aspiranteDto in aspirantesDtos)
            {
                if (!string.IsNullOrEmpty(aspiranteDto.IdAtendidoPorUsuario))
                {
                    var usuarioAtiende = await _authService.GetUserById(aspiranteDto.IdAtendidoPorUsuario);

                    aspiranteDto.UsuarioAtiendeNombre = ($"{usuarioAtiende.Nombres} {usuarioAtiende.Apellidos}").Trim();
                }
            }

            var response = new PagedResult<AspiranteDto>
            {
                TotalItems = pagination.TotalItems,
                Items = [.. aspirantesDtos],
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AspiranteDto>> GetById(int id)
        {
            var aspirante = await _aspiranteService.GetAspiranteById(id);

            var aspiranteDto = _mapper.Map<AspiranteDto>(aspirante);

            if (!string.IsNullOrEmpty(aspiranteDto.IdAtendidoPorUsuario))
            {
                var usuarioAtiende = await _authService.GetUserById(aspiranteDto.IdAtendidoPorUsuario);

                aspiranteDto.UsuarioAtiendeNombre = ($"{usuarioAtiende.Nombres} {usuarioAtiende.Apellidos}").Trim();
            }

            return Ok(aspiranteDto);
        }

        [HttpPost]
        public async Task<ActionResult<AspiranteDto>> Post([FromBody] AspiranteSignupRequest request)
        {
            Direccion? direccion = null;
            
            if (request.Calle != null && request.NumeroExterior != null && request.CodigoPostalId != null)
            {
                direccion = new Direccion
                {
                    Calle = request.Calle,
                    NumeroExterior = request.NumeroExterior,
                    NumeroInterior = request.NumeroInterior,
                    CodigoPostalId = request.CodigoPostalId.Value
                };
            }

            var newAspirante = new Aspirante
            {
                IdPersonaNavigation = new Persona
                {
                    Nombre = request.Nombre,
                    ApellidoPaterno = request.ApellidoPaterno,
                    ApellidoMaterno = request.ApellidoMaterno,
                    FechaNacimiento = request.FechaNacimiento,
                    IdGenero = request.GeneroId,
                    Curp = request.CURP,

                    Correo = request.Correo,
                    Telefono = request.Telefono,

                    IdDireccionNavigation = direccion,
                    IdEstadoCivil = request.IdEstadoCivil
                },
                IdPlan = request.PlanEstudiosId,
                IdMedioContacto = request.MedioContactoId,
                FechaRegistro = DateTime.UtcNow,
                Observaciones = request.Notas,
                TurnoId = request.HorarioId,
                IdAspiranteEstatus = request.AspiranteStatusId,
                IdAtendidoPorUsuario = request.AtendidoPorUsuarioId
            };

            try
            {
                var aspirante = await _aspiranteService.CrearAspirante(newAspirante);

                var aspiranteDto = _mapper.Map<AspiranteDto>(aspirante);

                return Ok(aspiranteDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] AspiranteUpdateRequest request)
        {
            Direccion? direccion = null;

            if (request.Calle != null && request.NumeroExterior != null && request.CodigoPostalId != null)
            {
                direccion = new Direccion
                {
                    Calle = request.Calle,
                    NumeroExterior = request.NumeroExterior,
                    NumeroInterior = request.NumeroInterior,
                    CodigoPostalId = request.CodigoPostalId.Value
                };
            }

            var newAspirante = new Aspirante
            {
                IdAspirante = request.AspiranteId,
                IdPersonaNavigation = new Persona
                {
                    Nombre = request.Nombre,
                    ApellidoPaterno = request.ApellidoPaterno,
                    ApellidoMaterno = request.ApellidoMaterno,
                    FechaNacimiento = request.FechaNacimiento,
                    IdGenero = request.GeneroId,
                    Curp = request.CURP,

                    Correo = request.Correo,
                    Telefono = request.Telefono,

                    IdDireccionNavigation = direccion,
                },
                IdPlan = request.PlanEstudiosId,
                IdMedioContacto = request.MedioContactoId,
                FechaRegistro = DateTime.UtcNow,
                Observaciones = request.Notas,
                TurnoId = request.HorarioId,
                IdAspiranteEstatus = request.AspiranteStatusId,
                IdAtendidoPorUsuario = request.AtendidoPorUsuarioId
            };

            try
            {
                await _aspiranteService.ActualizarAspirante(newAspirante);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("bitacora-seguimiento")]
        public async Task<ActionResult<IEnumerable<AspiranteSeguimientoDto>>> ObtenerBitacoraSeguimiento(int aspiranteId)
        {
            try
            {
                var bitacora = await _aspiranteService.GetBitacoraSeguimiento(aspiranteId);

                if (!bitacora.Any())
                {
                    return NoContent();
                }

                var bitacoraDto = _mapper.Map<IEnumerable<AspiranteSeguimientoDto>>(bitacora);

                foreach (var seguimiento in bitacoraDto)
                {
                    var usuarioAtiende = await _authService.GetUserById(seguimiento.UsuarioAtiendeId);

                    seguimiento.UsuarioAtiendeNombre = ($"{usuarioAtiende.Nombres} {usuarioAtiende.Apellidos}").Trim();
                }

                return Ok(bitacoraDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("bitacora-seguimiento")]
        public async Task<ActionResult<AspiranteSeguimientoDto>> CrearSeguimiento([FromBody] AspiranteSeguimientoRequest request)
        {
            var bitacoraSeguimiento = _mapper.Map<AspiranteBitacoraSeguimiento>(request);

            try
            {
                await _aspiranteService.CrearSeguimiento(bitacoraSeguimiento);

                var seguimientoDto = _mapper.Map<AspiranteSeguimientoDto>(bitacoraSeguimiento);

                return Ok(seguimientoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
