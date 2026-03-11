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
    public partial class EditarVehiculo
    {
        [Parameter] public string Matricula { get; set; } = string.Empty;

        private Boolean isAuthorized = false;

        private VehiculoUpdateRequest modeloEdicion = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                    if (string.IsNullOrEmpty(token))
                    {
                        Navigation.NavigateTo("/login");
                        return;
                    }

                    var request = new HttpRequestMessage(HttpMethod.Get, $"api/vehiculos/{Matricula}");
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    var response = await Http.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<ApiResponse<VehiculoResponse>>();
                        if (result != null && result.Success)
                        {
                            modeloEdicion.Modelo = result.Data.Modelo;
                            modeloEdicion.Marca = result.Data.Marca;
                            modeloEdicion.Precio = result.Data.Precio;
                            modeloEdicion.Kilometraje = result.Data.Kilometraje;
                            modeloEdicion.LitrosTanque = result.Data.LitrosTanque;

                            isAuthorized = true;
                            StateHasChanged(); 
                        }
                    }
                    else
                    {
                        await JS.InvokeVoidAsync("alert", "No se pudieron cargar los datos.");
                        Navigation.NavigateTo("/catalogo");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar: {ex.Message}");
                }
            }
        }

        public async Task editarVehiculo()
        {
            try
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var responseEditar = await Http.PutAsJsonAsync($"api/vehiculos/{Matricula}", modeloEdicion);

                if (responseEditar.IsSuccessStatusCode)
                {
                    await JS.InvokeVoidAsync("alert", "Vehículo actualizado correctamente");
                    Navigation.NavigateTo("/catalogo");
                }
                else if (responseEditar.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await JS.InvokeVoidAsync("alert", "Sesión expirada.");
                    Navigation.NavigateTo("/login");
                }
                else
                {
                    await JS.InvokeVoidAsync("alert", "Error al editar el vehículo.");
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", "Error de conexión.");
            }
        }

        private async Task Cancelar()
        {
            bool confirmado = await JS.InvokeAsync<bool>("confirm", "¿Estás seguro? Se perderán los cambios.");
            if (confirmado)
            {
                Navigation.NavigateTo("/catalogo");
            }
        }
    }
}
