namespace WebApplication2.Core.Requests.Auth
{
    public class CreateUserRequest : LoginRequest
    {
        public List<string> Roles { get; set; }
    }
}
