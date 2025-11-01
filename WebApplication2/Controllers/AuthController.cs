using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Models;
using WebApplication2.Core.Requests.Auth;
using WebApplication2.Services.Interfaces;

namespace WebApplication2
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IBlobStorageService blobStorageService, IConfiguration configuration, IMapper mapper)
        {
            _authService = authService;
            _blobStorageService = blobStorageService;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var userLoginInfoDto = await _authService.Login(request.Email, request.Password);

            var response = new Response<UserLoginInfoDto>
            {
                Data = userLoginInfoDto
            };

            return Ok(response);
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
            };

            var userLoginInfoDto = await _authService.Signup(user, request.Password, request.Roles);

            return NoContent();
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileRequest request, IFormFile? photoFile)
        {
            var newUser = new ApplicationUser
            {
                Email = request.Email,
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                Telefono = request.Telefono,
                Biografia = request.Biografia
            };

            var oldUser = await _authService.GetUserByEmail(request.Email);

            //var photoFileExtension = Path.GetExtension(photoFile.FileName);
            //var photoFileName = $"{oldUser.Id}/profile{photoFileExtension}";

            //var photoUrl = await _blobStorageService.UploadFile(photoFile, photoFileName, _configuration["Azure:PhotosContainerName"]);

            //newUser.PhotoUrl = photoUrl;

            try
            {
                var user = await _authService.UpdateUserProfile(newUser);

                var userDto = _mapper.Map<ApplicationUserDto>(user);

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                //await _blobStorageService.DeleteFile(photoFileName, _configuration["Azure:PhotosContainerName"]);

                throw new Exception("Error al guardar el usuario. " + ex.Message);
            }
        }
    }
}
