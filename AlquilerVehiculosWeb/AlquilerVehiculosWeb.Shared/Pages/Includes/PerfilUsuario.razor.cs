using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebApiNet.Shared.DTOs.Alquiler;
using WebApiNet.Shared.DTOs.Auth;
using WebApiNet.Shared.DTOs.Common;

namespace AlquilerVehiculosWeb.Shared.Pages.Includes
{
    public partial class PerfilUsuario
    {
        private UserResponse? userResponse;
        private List<AlquilerResponse> listadoAlquiler = new List<AlquilerResponse>();
        private bool mostrarAlquileres = false;

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
                    await ObtenerAlquileres();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener perfil: {ex.Message}");
                Navigation.NavigateTo("/login");
            }
        }

        private void Volver() => Navigation.NavigateTo("/catalogo");
    

        private async Task ObtenerAlquileres()
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
                
                var response = await Http.GetFromJsonAsync<ApiResponse<List<AlquilerResponse>>>("api/alquileres");
                
                if (response != null && response.Success)
                {
                    mostrarAlquileres = true;
                    listadoAlquiler = response.Data;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener alquileres: {ex.Message}");
            }
        }

        private async Task LlevarAlquiler()
        {
            var confirm = await JS.InvokeAsync<bool>("confirm", "¿Quieres ir al alquiler?");

            if (confirm)
            {
                Navigation.NavigateTo("/alquiler");
            }
        }
    }
}
