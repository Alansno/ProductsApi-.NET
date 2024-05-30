using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Context;
using ProductsApi.Custom;
using ProductsApi.Models;
using ProductsApi.Models.Dto;
using ProductsApi.Services.Interfaces;

namespace ProductsApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ProductContext _productContext;
        private readonly Utilities _utilities;

        public AuthService(ProductContext productContext, Utilities utilities)
        {
            _productContext = productContext;
            _utilities = utilities;
        }

        public async Task<string> Authenticate(LoginDto loginDto)
        {
            try
            {

                var userHash = await _productContext.Users
                .Where(u => u.Username == loginDto.Username)
                .FirstOrDefaultAsync();

                if (userHash == null) return "No existe el usuario";

                var hashedPass = _utilities.VerifyPassword(loginDto.Password,userHash.Password, userHash.Salt);

                if (hashedPass == false) return "Credenciales incorrectas";

                var userToken = _utilities.GenerateToken(userHash);
                return userToken;
            }
            catch (Exception ex) {
                throw new Exception("Algo salió mal", ex);
                Console.WriteLine(ex);
            }
        }

        public async Task<bool> RegisterUser(RegisterDto registerDto)
        {
            var userNew = new User
            {
                Email = registerDto.Email,
                Username = registerDto.Username,
                Password = _utilities.HashPassword(registerDto.Password, out byte[] salt),
                Salt = salt
            };

            try
            {
                await _productContext.Users.AddAsync(userNew);
                await _productContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Algo salió mal");
                Console.WriteLine($"{ex}");
            }

        }
    }
}
