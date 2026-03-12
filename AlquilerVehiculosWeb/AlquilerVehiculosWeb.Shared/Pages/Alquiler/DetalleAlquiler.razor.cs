using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebApiNet.Shared.DTOs.Alquiler;
using WebApiNet.Shared.DTOs.Common;

namespace AlquilerVehiculosWeb.Shared.Pages.Alquiler
{
    public partial class DetalleAlquiler
    {
        [Parameter]
        public string? Matricula { get; set; }

        private AlquilerResponse? detalleAlquiler;

        private bool isAuthorized = false;

        protected override async Task OnAfterRenderAsync(bool fistRender)
        {
            if (fistRender)
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                if (string.IsNullOrEmpty(token))
                {
                    Navigation.NavigateTo("/login");
                    return;
                }
                else
                {
                    isAuthorized = true;
                    await CargarDetalleAlquiler(token);
                    StateHasChanged();
                }
            }
        }

        private async Task CargarDetalleAlquiler(string token)
        {
            try
            {
                Http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.GetFromJsonAsync<ApiResponse<AlquilerResponse>>($"api/alquileres/{Matricula}");

                if (response != null && response.Success)
                {
                    detalleAlquiler = response.Data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el detalle del alquiler: {ex.Message}");
            }
        }

        private void IrAListadoAlquileres()
        {
            Navigation.NavigateTo("/alquiler");
        }
    }
}
