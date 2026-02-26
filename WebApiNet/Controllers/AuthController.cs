using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiNet.Constantes;
using WebApiNet.Dto;
using WebApiNet.Servicios;

namespace WebApiNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerDto)
        {
            try
            {
                var response = await _authService.Register(registerDto);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Registro exitoso",
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = response
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginDto)
        {
            try
            {
                var response = await _authService.Login(loginDto);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Login exitoso",
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = response
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow)
                });
            } catch (KeyNotFoundException ex)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow)
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado en el servidor"
                });
            }
        }

        [HttpGet("user")]
        [Authorize(Roles = RolesConstantes.AdminUser)]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var response = await _authService.GetUserAsync();
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Usuario encontrado",
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = response
                });

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow)
                });
            }
        }
    }
}