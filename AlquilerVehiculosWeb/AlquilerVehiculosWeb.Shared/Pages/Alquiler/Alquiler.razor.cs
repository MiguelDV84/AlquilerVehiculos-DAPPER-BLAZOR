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
using WebApiNet.Shared.Enums;
using WebApiNet.Shared.Paged;

namespace AlquilerVehiculosWeb.Shared.Pages.Alquiler
{
    public partial class Alquiler
    {
        // Lista de alquileres que se mostrarán en la página.
        private List<AlquilerResponse>? Listadoalquileres;

        private string textoBusqueda = "";
        // Switch booleano para controlar si el usuario tiene permiso de ver la página.
        private Boolean isAuthorized = false;

        // Variables para el modal de detalles del alquiler.
        private bool mostrarModalDetalle = false;
        private string matriculaSeleccionada = "";
        private AlquilerResponse? detalleAlquiler;

        //Variables para el modal de crear un nuevo alquiler
        private bool mostrarModalCrear = false;
        private AlquilerRequest nuevoAlquiler = new AlquilerRequest
        {
            FechaDevolucionPrevista = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
        };
        private List<VehiculoResponse>? ListadoVehiculos;
        private VehiculoResponse? vehiculoSeleccionado;
        private decimal PrecioPorDia = 0;

        //Variables para el modal de modificar alquiler
        private bool mostrarModalModificar = false;
        private AlquilerUpdateRequest updateRequest = new();
        private string? matriculaModificar;

        private List<AlquilerResponse>? AlquilerFiltrados =>
            string.IsNullOrWhiteSpace(textoBusqueda)
                ? Listadoalquileres
                : Listadoalquileres?.Where(a =>
                    a.VehiculoMatricula.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase)
                ).ToList();

        // Método auxiliar para obtener los datos del vehículo asociado a un alquiler.
        private VehiculoResponse? ObtenerDatosVehiculo(string matricula)
        {
            return ListadoVehiculos?.FirstOrDefault(v =>
                v.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase));
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var MiToken = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                if (!string.IsNullOrEmpty(MiToken))
                {
                    isAuthorized = true;
                    
                    await CargarAlquileres(MiToken);
                    await CargarVehiculos(MiToken);
                    // Notificamos a Blazor que el estado ha cambiado para que pinte los coches.
                    StateHasChanged();
                }
                else
                {
                    // Si no hay token, redirigimos al login.
                    Navigation.NavigateTo("/login");
                }
            }
        }

        private async Task CargarAlquileres(String Token)
        {
            try
            {
                // Configuramos el HttpClient para incluir el token en la cabecera de autorización.
                Http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

                // Hacemos la petición GET a la API para obtener los alquileres.
                var result = await Http.GetFromJsonAsync<ApiResponse<List<AlquilerResponse>>>("api/alquileres");
                if (result != null && result.Success)
                {
                    // Si la respuesta es exitosa, asignamos los datos a la variable que se muestra en la UI.
                    Listadoalquileres = result.Data;
                }
                else
                {
                    // Si la respuesta no es exitosa, mostramos una alerta con el mensaje de error.
                    await JS.InvokeVoidAsync("alert", $"Error al cargar alquileres: {result?.Message ?? "Respuesta vacía"}");
                }
            }
            catch (Exception ex)
            {
                // En caso de error, mostramos una alerta y redirigimos al login (posible token expirado).
                await JS.InvokeVoidAsync("alert", $"Error al cargar alquileres: {ex.Message}");
                Navigation.NavigateTo("/");
            }
        }

        private async Task FinalizarAquiler(String matricula)
        {
            bool confirm = await JS.InvokeAsync<bool>("confirm", $"¿Estás seguro de finalizar el alquiler del vehículo {matricula}?");

            if (confirm)
            {
                try
                {
                    var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                    Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    var response = await Http.PutAsync($"api/alquileres/finalizar/{matricula}", null);

                    if (response.IsSuccessStatusCode)
                    {
                        await JS.InvokeVoidAsync("alert", $"Alquiler del vehículo {matricula} finalizado correctamente.");
                        await CargarAlquileres(token);
                        StateHasChanged();
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        await JS.InvokeVoidAsync("alert", $"Error al finalizar alquiler: {response.ReasonPhrase} - {errorContent}");
                    }

                }
                catch (Exception ex)
                {
                    await JS.InvokeVoidAsync("alert", $"Error al finalizar alquiler: {ex.Message}");
                }
            }
        }

        private async Task MostrarDetalle(string matricula)
        {

            matriculaSeleccionada = matricula;
            detalleAlquiler = null;
            mostrarModalDetalle = true;

            await CargarDatosDetalle(matricula);
        }

        private async Task CargarDatosDetalle(string matricula)
        {
            try
            {

                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.GetFromJsonAsync<ApiResponse<AlquilerResponse>>($"api/alquileres/{matricula}");
                if (response != null && response.Success)
                {
                    detalleAlquiler = response.Data;
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", $"Error al cargar detalle del alquiler: {ex.Message}");
            }
        }

        private async Task MostrarModalCrear()
        {
            try
            {

                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                
                if (token != null)
                {
                    nuevoAlquiler = new AlquilerRequest
                    {
                        FechaDevolucionPrevista = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
                    };

                    vehiculoSeleccionado = null;
                    PrecioPorDia = 0;
                    await CargarVehiculos(token);
                    mostrarModalCrear = true;
                }
            } catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", $"Error al cargar vehículos: {ex.Message}");
            }
        }

        private async Task CerrarModal()
        {
            var confirmacion = await JS.InvokeAsync<bool>("confirm", "¿Estás seguro de que deseas cancelar?");

            if (confirmacion)
            {
                mostrarModalDetalle = false;
                mostrarModalCrear = false;
                mostrarModalModificar = false;
            }
        }

        private async Task CargarVehiculos(string token)
        {
            try
            {
                Http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Pedimos todos los vehículos (sin filtrar por estado aquí)
                var response = await Http.GetAsync("api/vehiculos?pageNumber=1&pageSize=100");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<VehiculoResponse>>>();
                    if (result?.Data?.Items != null)
                    {
                        // Guardamos TODOS para que las tarjetas de alquileres puedan mostrar Marca/Modelo
                        ListadoVehiculos = result.Data.Items.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", $"Error al cargar vehículos: {ex.Message}");
            }
        }

        private async Task VehiculoSeleccionado(ChangeEventArgs e)
        {
            var matricula = e.Value?.ToString();
            if (!string.IsNullOrEmpty(matricula))
            {
                nuevoAlquiler.VehiculoMatricula = matricula;

                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                var result = await Http.GetFromJsonAsync<ApiResponse<VehiculoResponse>>($"api/vehiculos/{matricula}");
                if (result != null && result.Success)
                {
                    vehiculoSeleccionado = result.Data;
                    PrecioPorDia = result.Data.Precio;
                }
            }
            else
            {
                vehiculoSeleccionado = null;
                PrecioPorDia = 0;
                nuevoAlquiler.VehiculoMatricula = string.Empty;
            }
        }

        private async Task EjecutarCrearAlquiler()
        {
            try
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.PostAsJsonAsync("api/alquileres", nuevoAlquiler);

                if (response.IsSuccessStatusCode)
                {
                    await JS.InvokeVoidAsync("alert", "Alquiler creado con éxito.");
                    mostrarModalCrear = false; // Cerramos modal
                    await CargarAlquileres(token); // Refrescamos la lista de la pantalla principal
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await JS.InvokeVoidAsync("alert", $"Error: {error}");
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", $"Error al procesar: {ex.Message}");
            }
        }

        private decimal TotalCalculado
        {
            get
            {
                if (vehiculoSeleccionado == null) return 0;

                var fechaInicio = DateOnly.FromDateTime(DateTime.UtcNow).DayNumber;
                var fechaFin = nuevoAlquiler.FechaDevolucionPrevista.DayNumber;

                int dias = (fechaFin - fechaInicio);

                if (dias <= 0) dias = 1;

                return dias * vehiculoSeleccionado.Precio;
            }
        }

        private async Task MostrarModalModificar(string matricula)
        { 
            matriculaModificar = matricula;
            updateRequest = new AlquilerUpdateRequest();
            mostrarModalModificar = true;
            
            await ObtenerAlquilerParaEdicion(matricula);

        }

        private async Task ObtenerAlquilerParaEdicion(string matricula)
        {
            try
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.GetFromJsonAsync<ApiResponse<AlquilerResponse>>($"api/alquileres/{matricula}");

                if (response != null && response.Success)
                {
                    detalleAlquiler = response.Data;

                    updateRequest.FechaDevolucionPrevista = response.Data.FechaDevolucionPrevista;
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", $"Error: {ex.Message}");
            }
        }

        private async Task EjecutarModificarAlquiler()
        {
            try
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                Http.DefaultRequestHeaders.Authorization = null;
                Http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.PutAsJsonAsync($"api/alquileres/{matriculaModificar}", updateRequest);

                if (response.IsSuccessStatusCode)
                {
                    await JS.InvokeVoidAsync("alert", "Alquiler modificado con éxito.");
                    mostrarModalModificar = false; 
                    await CargarAlquileres(token); 
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await JS.InvokeVoidAsync("alert", $"Error: {error}");
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", $"Error al modificar alquiler: {ex.Message}");
            }

        }
    }
}
