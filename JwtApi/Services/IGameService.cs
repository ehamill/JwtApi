using JwtApi.Entities;
using JwtApi.Models;

namespace JwtApi.Services
{
    public interface IGameService
    {
        //Task<User?> RegisterAsync(UserDto request);
        //Task<string> LoginAsync(UserDto request);
        Task<City> GetCity(string UserID);
    }
}
