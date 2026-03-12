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
using WebApiNet.Shared.DTOs.Vehiculo;
using WebApiNet.Shared.Paged;

namespace AlquilerVehiculosWeb.Shared.Pages.Alquiler
{
    public partial class NuevoAlquiler
    {
        private AlquilerRequest nuevoAlquiler = new AlquilerRequest
        {
            FechaDevolucionPrevista = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
        };
        private List<VehiculoResponse>? ListadoVehiculos;
        private VehiculoResponse? vehiculoSeleccionado;

        private decimal PrecioPorDia = 0;

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
                    await CargarVehiculos(token);
                    StateHasChanged();
                }
            }
        }

        private async Task CargarVehiculos(string token)
        {
            try
            {
                Http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.GetAsync("api/vehiculos?pageNumber=1&pageSize=100");

                if (response.IsSuccessStatusCode)
                {
                    // CAMBIO CLAVE: Usamos PagedResult igual que en el catálogo
                    var result = await response.Content
                        .ReadFromJsonAsync<ApiResponse<PagedResult<VehiculoResponse>>>();

                    if (result?.Data?.Items != null)
                    {
                        // Extraemos solo la lista de ítems para el dropdown
                        ListadoVehiculos = result.Data.Items.Where(v => v.Estado == 0).ToList();
                        StateHasChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar vehículos: {ex.Message}");
            }
        }

        private async Task VehiculoSeleccionado(ChangeEventArgs e)
        {
            var matricula = e.Value?.ToString();
            if (!string.IsNullOrEmpty(matricula))
            {
                nuevoAlquiler.VehiculoMatricula = matricula;
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                await DetallesVehiculo(matricula, token);
            }
            else
            {
                vehiculoSeleccionado = null;
                PrecioPorDia = 0;
            }
        }

        private decimal CalcularPrecioTotal()
        {
            DateTime fechaPrevista = nuevoAlquiler.FechaDevolucionPrevista.ToDateTime(TimeOnly.MinValue);
            DateTime hoy = DateTime.Now.Date;

            var dias = (fechaPrevista - hoy).Days;

            return dias * PrecioPorDia;
        }

        private async Task DetallesVehiculo(string matricula, string token)
        {
            try
            {
                Http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var resultado = await Http.GetFromJsonAsync<ApiResponse<VehiculoResponse>>($"api/vehiculos/{matricula}");

                if (resultado != null && resultado.Success)
                {
                    vehiculoSeleccionado = resultado.Data;
                    PrecioPorDia = resultado.Data.Precio;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener detalles: {ex.Message}");
            }
        }


        private async Task CrearAlquilerNuevo()
        {
            try
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                if (string.IsNullOrEmpty(token))
                {
                    await JS.InvokeVoidAsync("alert", "Debes estar logueado para alquilar.");
                    return;
                }

                Http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.PostAsJsonAsync("api/alquileres", nuevoAlquiler);

                if (response.IsSuccessStatusCode)
                {
                    await JS.InvokeVoidAsync("alert", "Alquiler creado con éxito.");
                    Navigation.NavigateTo("/alquiler");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await JS.InvokeVoidAsync("alert", "Sesión no válida. Por favor, inicia sesión.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await JS.InvokeVoidAsync("alert", $"Error al crear el alquiler: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", $"Error al crear el alquiler: {ex.Message}");
            }
        }

        private async Task Cancelar()
        {
            var confirmacion = await JS.InvokeAsync<bool>("confirm", "¿Estás seguro de que deseas cancelar?");

            if (confirmacion)
            {
                Navigation.NavigateTo("/alquiler");
            }
        }
    }
}
