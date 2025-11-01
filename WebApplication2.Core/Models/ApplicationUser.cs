using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Biografia { get; set; }
        public string PhotoUrl { get; set; }
    }
}
