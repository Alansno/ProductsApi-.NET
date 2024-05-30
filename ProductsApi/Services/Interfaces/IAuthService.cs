using ProductsApi.Models;
using ProductsApi.Models.Dto;

namespace ProductsApi.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<bool> RegisterUser(RegisterDto registerDto);
        public Task<string> Authenticate(LoginDto loginDto);
    }
}
