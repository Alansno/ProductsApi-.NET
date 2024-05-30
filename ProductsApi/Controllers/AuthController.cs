using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Models;
using ProductsApi.Models.Dto;
using ProductsApi.Services;
using ProductsApi.Services.Interfaces;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("registrate")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var userResult = await _authService.RegisterUser(registerDto);

                if (userResult != false)
                {
                    return StatusCode(StatusCodes.Status201Created, new { isSuccess = true });
                }

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
            }
            catch (ArgumentNullException ex) { 
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("Iniciar-sesion")]
        public async Task<IActionResult> AuthenticateUser([FromBody] LoginDto loginDto)
        {
            try
            {
                var authResul = await _authService.Authenticate(loginDto);

                if (authResul == "No existe el usuario" && authResul == "Credenciales incorrectas")
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
                }

                return Ok(new {token  = authResul});
            }
            catch (Exception ex) {
                Console.WriteLine($"Error en AuthenticateUser: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

    }
}
