using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.Configuration.Constants;
using WebApplication2.Configuration.CustomExceptions;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Models;
using WebApplication2.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication2.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<ApplicationUser> Signup(ApplicationUser user, string password, List<string> roles)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(" ", result.Errors.Select(error => error.Description));

                throw new ValidationException(errors);
            }

            foreach (var role in roles)
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
                await _userManager.AddToRoleAsync(user, role);
            }
            
            return user;

        }

        public async Task<UserLoginInfoDto> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) throw new ValidationException(ErrorConstants.INVALID_CREDENTIALS);

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);

            if (!result.Succeeded)
            {
                throw new ValidationException(ErrorConstants.INVALID_CREDENTIALS);
            }

            var roles = await _userManager.GetRolesAsync(user);

            var userLoginTokenDto = GetUserLoginToken(user, roles.FirstOrDefault()!);

            return userLoginTokenDto;

        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            return user;
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            return user;
        }

        public async Task RequestPasswordReset(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var applicationUrl = _configuration["ApplicationUrl"];

            var callbackUrl = $"{applicationUrl}/api/auth/password-reset/{passwordResetToken}";

            // await SendTokenByEmail(user.Email, callbackUrl);
        }

        public async Task ResetPassword(string email, string newPassword, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            await _userManager.ResetPasswordAsync(user, newPassword, token);
        }

        public async Task<ApplicationUser> UpdateUserProfile(ApplicationUser newUser)
        {
            var user = await _userManager.FindByEmailAsync(newUser.Email);

            user.Nombres = newUser.Nombres;
            user.Apellidos = newUser.Apellidos;
            user.Telefono = newUser.Telefono;
            user.Biografia = newUser.Biografia;
            user.PhotoUrl = newUser.PhotoUrl;

            await _userManager.UpdateAsync(user);

            return user;
        }

        public async Task DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            await _userManager.DeleteAsync(user);
        }

        private UserLoginInfoDto GetUserLoginToken(ApplicationUser user, string role)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var expiration = DateTime.UtcNow.AddHours(1);

            var token = BuildToken(claims, expiration);

            return new UserLoginInfoDto()
            {
                UserId = user.Id,
                Email = user.Email,
                Nombres = user.Nombres,
                Apellidos = user.Apellidos,
                Telefono = user.Telefono,
                Biografia = user.Biografia,
                Role = role,
                Token = token,
                Expiration = expiration,
                PhotoUrl = user.PhotoUrl,
            };
        }

        private string BuildToken(List<Claim> claims, DateTime expiration)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //private async Task SendTokenByEmail(string to, string callbackUrl)
        //{
        //    var email = new Email
        //    {
        //        To = to,
        //        Body = callbackUrl,
        //        Subject = "Request Password Reset"
        //    };

        //    await _emailService.SendAsync(email);
        //}
    }
}
