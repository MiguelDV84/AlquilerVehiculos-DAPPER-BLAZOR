using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiNet.Core.Interfaces;
using WebApiNet.Presentation.Constants;
using WebApiNet.Shared.DTOs.Alquiler;
using WebApiNet.Shared.DTOs.Common;

namespace WebApiNet.Controllers
{
   /* [ApiController]
    [Route("api/[controller]")]
   public class AlquilerController : ControllerBase
    {
        private readonly IAlquilerService _alquilerService;

        public AlquilerController(IAlquilerService alquilerService)
        {
            _alquilerService = alquilerService;
        }

        [HttpPost]
        [Authorize(Roles = RolesConstantes.AdminUser)]
        public async Task<IActionResult> CreateAlquiler([FromBody] AlquilerRequest request)
        {
            try
            {
                var alquiler = await _alquilerService.CreateAlquilerAsync(request);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Alquiler creado correctamente",
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = alquiler
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = null
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = null
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = RolesConstantes.AdminUser)]
        public async Task<IActionResult> GetAllAlquileres()
        {
            try
            {
                var alquileres = await _alquilerService.GetAllAlquileresAsync();
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Alquileres obtenidos correctamente",
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = alquileres
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = null
                });
            }
        }

        [HttpGet("{vehiculoMatricula}")]
        [Authorize(Roles = RolesConstantes.AdminUser)]
        public async Task<IActionResult> GetAlquilerByMatricula([FromRoute] string vehiculoMatricula)
        {
            try
            {
                var alquiler = await _alquilerService.GetAlquilerDtoAsync(vehiculoMatricula);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Alquiler obtenido correctamente",
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = alquiler
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = null
                });
            } 
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = null
                });
            }
        }

        [HttpPut("finalizar/{vehiculoMatricula}")]
        [Authorize(Roles = RolesConstantes.AdminUser)]
        public async Task<IActionResult> FinishAlquiler([FromRoute] string vehiculoMatricula)
        {
            try
            {
                var alquiler = await _alquilerService.FinishAlquilerAsync(vehiculoMatricula);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Alquiler finalizado",
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = alquiler
                });
            } catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = null
                });
            }
        }
    }*/
}
