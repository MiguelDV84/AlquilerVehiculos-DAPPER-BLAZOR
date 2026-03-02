using Microsoft.AspNetCore.Diagnostics;
using WebApiNet.Application.DTOs.Common;
using WebApiNet.Core.Exceptions;

namespace WebApiNet.Presentation.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
        {
            // 1. Declaramos las variables de la tupla y forzamos el tipo en la primera rama
            // para evitar el error de "No se encontró el mejor tipo".
            (int statusCode, string message) = exception switch
            {
                DuplicateEntityException duplicate =>
                    (StatusCodes.Status409Conflict, duplicate.Message),

                NotFoundException notFound =>
                    (StatusCodes.Status404NotFound, notFound.Message),

                _ => (StatusCodes.Status500InternalServerError, "Ha ocurrido un error inesperado")
            };

            // 2. Configuramos la respuesta
            httpContext.Response.StatusCode = statusCode;

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = message
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            // 3. Devolvemos true para indicar que la excepción ya fue manejada
            return true;
        }
    }
}
