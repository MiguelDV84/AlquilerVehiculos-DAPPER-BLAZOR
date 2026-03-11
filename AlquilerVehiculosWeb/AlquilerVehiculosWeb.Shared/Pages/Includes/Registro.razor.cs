using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebApiNet.Shared.DTOs.Auth;
using WebApiNet.Shared.DTOs.Common;

namespace AlquilerVehiculosWeb.Shared.Pages.Includes
{
    public partial class Registro
    {
        private RegisterRequest registrarRequest = new();

        private bool isLoading = false;

        private async Task CrearUsuario()
        {
            try
            {
                isLoading = true;

                var response = await Http.PostAsJsonAsync("api/auth/register", registrarRequest);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();

                    if (result != null && result.Success)
                    {
                        await JS.InvokeVoidAsync("alert", "Usuario creado exitosamente. Ahora puedes iniciar sesión.");
                        Navigation.NavigateTo("/login");
                    }
                }
                else
                {
                    var errorBody = await response.Content.ReadAsStringAsync();

                    try
                    {
                        var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
                        if (errorResult != null && !string.IsNullOrEmpty(errorResult.Message))
                        {
                            await JS.InvokeVoidAsync("alert", $"Error: {errorResult.Message}");
                        }
                        else
                        {
                            await JS.InvokeVoidAsync("alert", "Error al crear usuario.");
                        }
                    }
                    catch
                    {
                        await JS.InvokeVoidAsync("alert", "Error en el servidor. Verifica los datos.");
                    }
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", $"Ocurrió un error: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task Cancelar()
        {
            var confirmacion = await JS.InvokeAsync<bool>("confirm", "¿Estás seguro? Se perderán los datos.");

            if (confirmacion)
            {
                Navigation.NavigateTo("/login");
            }
        }

        protected override async Task OnAfterRenderAsync(Boolean firstRender)
        {
            if (firstRender)
            {
                var miToken = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                if (!string.IsNullOrEmpty(miToken))
                {
                    Navigation.NavigateTo("/catalogo");
                }
            }
        }
    }
}
