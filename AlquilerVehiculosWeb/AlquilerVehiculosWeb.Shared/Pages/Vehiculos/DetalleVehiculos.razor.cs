using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebApiNet.Shared.DTOs.Common;
using WebApiNet.Shared.DTOs.Vehiculo;

namespace AlquilerVehiculosWeb.Shared.Pages.Vehiculos
{
    public partial class DetalleVehiculos
    {
        [Parameter]
        public string Matricula { get; set; } = string.Empty;

        private VehiculoResponse? vehiculo;

        private bool isAuthorized = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                if (string.IsNullOrEmpty(token))
                {
                    Navigation.NavigateTo("/login");
                }
                else
                {
                    isAuthorized = true;
                    await CargarDetalles(token);

                    StateHasChanged();
                }
            }
        }

        private async Task CargarDetalles(string token)
        {
            try
            {
                Http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.GetFromJsonAsync<ApiResponse<VehiculoResponse>>($"api/vehiculos/{Matricula}");

                if (response != null && response.Success)
                {
                    vehiculo = response.Data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar detalles: {ex.Message}");
            }
        }

        private void Volver() => Navigation.NavigateTo("/catalogo");
    }
}
