using WebApiNet.Shared.DTOs.Auth;
using WebApiNet.Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace AlquilerVehiculosWeb.Shared.Pages.Includes
{
    public partial class Login
    {
        private LoginRequest loginModel = new();

        private Boolean isProcessing = false;

        private async Task RealizarLogin()
        {
            isProcessing = true;
            try
            {

                var response = await Http.PostAsJsonAsync("api/auth/login", loginModel);

                if (response.IsSuccessStatusCode)
                {

                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();

                    if (result != null && result.Success)
                    {

                        await JS.InvokeVoidAsync("localStorage.setItem", "token", result.Data.Token);
                        await JS.InvokeVoidAsync("localStorage.setItem", "usuarioNombre", result.Data.User.Nombre);

                        Navigation.NavigateTo("/", forceLoad: true);
                    }
                }
                else
                {
                    // Manejo de errores (Credenciales incorrectas o usuario no encontrado).
                    await JS.InvokeVoidAsync("alert", "Error al iniciar sesión. Verifica tus credenciales.");
                }
            }
            catch (Exception ex)
            {
                // Error de conexión o fallo inesperado del servidor.
                await JS.InvokeVoidAsync("alert", $"Ocurrió un error: {ex.Message}");
            }
            finally
            {
                isProcessing = false; // Reactivamos el botón independientemente del resultado.
            }
        }

        // --- SINCRONIZACIÓN DE ESTADO ---
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var miToken = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                if (!string.IsNullOrEmpty(miToken))
                {
                    //Si ya hay un token, redirigimos al usuario a la página principal.
                    Navigation.NavigateTo("/perfil");

                }
            }
        }
    }
}
