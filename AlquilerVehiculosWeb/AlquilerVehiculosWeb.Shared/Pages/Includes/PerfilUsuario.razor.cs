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
    public partial class PerfilUsuario
    {
        private UserResponse? userResponse;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ObtenerDatosUsuario();
            }
        }

        private async Task ObtenerDatosUsuario()
        {
            try
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                if (string.IsNullOrEmpty(token))
                {
                    Navigation.NavigateTo("/login");
                    return;
                }

                Http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.GetFromJsonAsync<ApiResponse<UserResponse>>("api/auth/user");

                if (response != null && response.Success)
                {
                    userResponse = response.Data;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener perfil: {ex.Message}");
                Navigation.NavigateTo("/login");
            }
        }

        private void Volver() => Navigation.NavigateTo("/catalogo");
    }
}
