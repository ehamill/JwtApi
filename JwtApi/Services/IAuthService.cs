using JwtApi.Entities;
using JwtApi.Models;

namespace JwtApi.Services
{
    public interface IAuthService
    {
       Task<User?> RegisterAsync(UserDto request);
       Task<string> LoginAsync(UserDto request);

    }
}
