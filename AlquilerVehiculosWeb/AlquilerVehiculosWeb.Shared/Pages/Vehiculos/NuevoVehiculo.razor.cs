using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebApiNet.Shared.DTOs.Vehiculo;

namespace AlquilerVehiculosWeb.Shared.Pages.Vehiculos
{
    public partial class NuevoVehiculo
    {

        private bool isAuthorized = false;
        private VehiculoRequest nuevoVehiculo = new();
        private bool validandoMatricula = false;
        private string mensajeErrorMatricula = "";

        private async Task ValidarMatricula()
        {
            if (string.IsNullOrWhiteSpace(nuevoVehiculo.Matricula)) return;

            validandoMatricula = true;
            mensajeErrorMatricula = "";

            try
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                Http.DefaultRequestHeaders.Authorization = null;
                Http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.GetAsync($"api/vehiculos/{nuevoVehiculo.Matricula}");

                if (response.IsSuccessStatusCode)
                {
                    mensajeErrorMatricula = "Esta matrícula ya está registrada.";
                }
            }
            catch (Exception)
            {
                mensajeErrorMatricula = "";
            }
            finally
            {
                validandoMatricula = false;
                StateHasChanged();
            }
        }

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
                    StateHasChanged(); 
                }
            }
        }

        private async Task GuardarVehiculo()
        {
            try
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                Http.DefaultRequestHeaders.Authorization = null;
                Http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.PostAsJsonAsync("api/vehiculos", nuevoVehiculo);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await JS.InvokeVoidAsync("alert", $"Error al guardar: {errorContent}");
                }
                
                if (response.IsSuccessStatusCode)
                {
                    await JS.InvokeVoidAsync("alert", "Vehículo creado exitosamente");
                    Navigation.NavigateTo("/catalogo");
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await JS.InvokeVoidAsync("alert", "Sesión no válida. Por favor, inicia sesión.");
                    Navigation.NavigateTo("/login");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                await JS.InvokeVoidAsync("alert", "Error de conexión con el servidor.");
            }
        }

        private async Task Cancelar()
        {
            // Validación de salida para no perder datos por un clic accidental.
            var confirmacion = await JS.InvokeAsync<bool>("confirm", "¿Deseas cancelar? Se perderán los datos introducidos.");

            if (confirmacion)
            {
                Navigation.NavigateTo("/catalogo");
            }
        }
    }
}
