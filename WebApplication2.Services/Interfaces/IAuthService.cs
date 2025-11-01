using WebApplication2.Core.DTOs;
using WebApplication2.Core.Models;


namespace WebApplication2.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApplicationUser> Signup(ApplicationUser user, string password, List<string> roles);
        Task<UserLoginInfoDto> Login(string username, string password);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<ApplicationUser> GetUserById(string id);
        Task RequestPasswordReset(string email);
        Task ResetPassword(string email, string newPassword, string token);
        Task<ApplicationUser> UpdateUserProfile(ApplicationUser newUser);
        Task DeleteUser(string email);
    }
}
