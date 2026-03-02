using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiNet.Constantes;
using WebApiNet.Dto;
using WebApiNet.Servicios;

namespace WebApiNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculoController : ControllerBase
    {
        private readonly IVehiculoService _vehiculoService;


        public VehiculoController(IVehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        [HttpGet]
        [Authorize(Roles = RolesConstantes.AdminUser)]
        public async Task<IActionResult> GetVehiculos()
        {
            var vehiculos = await _vehiculoService.GetAllVehiculosAsync();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Vehiculos obtenidos correctamente",
                Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                Data = vehiculos
            });
        }

        [HttpGet("{matricula}")]
        [Authorize(Roles = RolesConstantes.AdminUser)]
        public async Task<IActionResult> GetVehiculo([FromRoute] string matricula)
        {
            try
            {
                var vehiculos = await _vehiculoService.GetVehiculoByMatriculaAsync(matricula);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Vehiculo encontrado",
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = vehiculos
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = null
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

        [HttpPost]
        [Authorize(Roles = RolesConstantes.Admin)]
        public async Task<IActionResult> Insert([FromBody] VehiculoRequest vehiculoDto)
        {
            try
            {
                var vehiculo = await _vehiculoService.CreateVehiculoAsync(vehiculoDto);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Vehiculo insertado correctamente",
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = vehiculo
                });
            } catch(InvalidOperationException ex)
            {
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = ex.Message,
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = null
                });
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
            
        }

        [HttpDelete("{matricula}")]
        [Authorize(Roles = RolesConstantes.Admin)]
        public async Task<IActionResult> Delete([FromRoute] string matricula)
        {
            try
            {
                await _vehiculoService.DeleteVehiculoAsync(matricula);
                return Ok(new
                {
                    success = true,
                    message = "Vehiculo eliminado correctamente",
                    timestamp = DateTime.UtcNow
                });

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            } catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpPut("editar/{matricula}")]
        [Authorize(Roles = RolesConstantes.Admin)]
        public async Task<IActionResult> Editar([FromRoute] string matricula, [FromBody] VehiculoUpdateRequest vehiculoDto)
        {
            try
            {
                var vehiculo = await _vehiculoService.UpdateVehiculoAsync(matricula, vehiculoDto);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Vehiculo editado correctamente",
                    Timestamp = DateOnly.FromDateTime(DateTime.UtcNow),
                    Data = vehiculo
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
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }


       

    }
}
