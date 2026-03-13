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
using WebApiNet.Shared.Paged;

namespace AlquilerVehiculosWeb.Shared.Pages.Vehiculos
{
    public partial class Catalogo
    {
        private List<VehiculoResponse>? listaDeVehiculo;

        private string textoBusqueda = "";

        private bool isAuthorized = false;

        private int currentPage = 1;
        private int pageSize = 15;
        private int totalPages = 1;
        private int totalCount = 0;

        public string Matricula { get; set; } = string.Empty;

        private List<VehiculoResponse>? VehiculoFiltrados =>
            string.IsNullOrWhiteSpace(textoBusqueda)
                ? listaDeVehiculo
                : listaDeVehiculo?.Where(v =>
                    v.Marca.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                    v.Modelo.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase)
                  ).ToList();

        //Variable para el modal de detalles de vehículo
        private bool mostrarModalDetalles = false;
        private VehiculoResponse? vehiculoSeleccionado;


        // Variable para el modal de crear un nuevo vehículo
        private bool mostrarModalCrear = false;
        private VehiculoRequest nuevoVehiculo = new();
        private bool validandoMatricula = false;
        private string mensajeErrorMatricula = "";

        // Variable para el modal de editar vehículo
        private bool mostrarModalModificar = false;
        private VehiculoUpdateRequest? modeloEdicion = new();
        private string? matriculaEnEdicion;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;

            var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

            if (!string.IsNullOrEmpty(token))
            {

                isAuthorized = true;
                await CargarVehiculos(token);

                StateHasChanged();
            }
            else
            {
                Navigation.NavigateTo("/login");
            }
        }

        private async Task IrAPaginaDirecto(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int paginaDestino))
            {
                // Validamos que la página esté dentro del rango real
                if (paginaDestino >= 1 && paginaDestino <= totalPages)
                {
                    currentPage = paginaDestino;
                    var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                    await CargarVehiculos(token);
                }
                else
                {
                    // Si pone una página que no existe, reseteamos el input al valor actual
                    StateHasChanged();
                }
            }
        }

        private async Task CambiarTamañoPagina(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int nuevoTamaño))
            {
                pageSize = nuevoTamaño;
                currentPage = 1; // Reiniciamos a la primera página al cambiar el tamaño
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                await CargarVehiculos(token);
            }
        }

        private async Task CargarVehiculos(string token)
        {
            try
            {
                Http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.GetAsync($"api/vehiculos?pageNumber={currentPage}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content
                        .ReadFromJsonAsync<ApiResponse<PagedResult<VehiculoResponse>>>();

                    if (result?.Data != null)
                    {
                        listaDeVehiculo = result.Data.Items.ToList();
                        totalCount = result.Data.TotalCount;
                        totalPages = result.Data.TotalPages;
                        currentPage = result.Data.PageNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", ex.Message);
            }
        }

        //private void EditarVehiculo(string matricula) => Navigation.NavigateTo($"/editar-vehiculo/{matricula}");

        private async Task BorrarVehiculo(string matricula)
        {
            bool confirmado = await JS.InvokeAsync<bool>("confirm", $"¿Borrar vehículo {matricula}?");

            if (!confirmado) return;

            try
            {
                // 2. Obtener token
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                if (string.IsNullOrEmpty(token))
                {
                    await JS.InvokeVoidAsync("alert", "Sesión expirada. Identifíquese de nuevo.");
                    Navigation.NavigateTo("/login");
                    return;
                }

                // 3. Configurar el cliente (Limpiar y poner token nuevo)
                Http.DefaultRequestHeaders.Authorization = null;
                Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // 4. Ejecutar DELETE
                var response = await Http.DeleteAsync($"api/vehiculos/{matricula}");

                if (response.IsSuccessStatusCode)
                {
                    // 5. Feedback y Recarga
                    await JS.InvokeVoidAsync("alert", "Vehículo eliminado correctamente.");
                    await CargarVehiculos(token);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await JS.InvokeVoidAsync("alert", $"No se pudo borrar: {error}");
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", $"Error de conexión: {ex.Message}");
            }
        }

        private async Task PaginaAnterior()
        {
            if (currentPage > 1)
            {
                currentPage--;
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                await CargarVehiculos(token);
            }
        }

        private async Task PaginaSiguiente()
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                await CargarVehiculos(token);
            }
        }

        // Métodos para el modal de detalles
        private async Task MostrarDetalle(string matricula)
        {
            mostrarModalDetalles = true; 
            vehiculoSeleccionado = null;  

            await ObtenerVehiculoDeApi(matricula); 
        }

        
        private async Task ObtenerVehiculoDeApi(string matricula)
        {
            try
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.GetFromJsonAsync<ApiResponse<VehiculoResponse>>($"api/vehiculos/{matricula}");
                if (response != null && response.Success)
                {
                    vehiculoSeleccionado = response.Data;
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", "Error al conectar con el servidor");
            }
        }

        private async Task CerrarModal()
        {
            bool confirmado = await JS.InvokeAsync<bool>("confirm", "¿Volver al catálogo?");

            if (confirmado)
            {
                mostrarModalDetalles = false;
                mostrarModalCrear = false;
                mostrarModalModificar = false;
            }
        }

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

        private async Task MostrarModalCrear()
        {
            nuevoVehiculo = new VehiculoRequest(); 
            mostrarModalCrear = true;
        }

        private async Task GuardarNuevoVehiculo()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nuevoVehiculo.Matricula) || !string.IsNullOrEmpty(mensajeErrorMatricula))
                {
                    await JS.InvokeVoidAsync("alert", "Por favor, corrija los errores antes de guardar.");
                    return;
                }

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
                    mostrarModalCrear = false; 
                    await CargarVehiculos(token!); 
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

        private async Task MostrarModalModificar(string matricula)
        {
            matriculaEnEdicion = matricula; 
            modeloEdicion = new VehiculoUpdateRequest();
            mostrarModalModificar = true;

            await ObtenerVehiculoParaEdicion(matricula);
        }

        private async Task ObtenerVehiculoParaEdicion(string matricula)
        {
            try
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await Http.GetFromJsonAsync<ApiResponse<VehiculoResponse>>($"api/vehiculos/{matricula}");
                if (response != null && response.Success)
                {
                    modeloEdicion.Modelo = response.Data.Modelo;
                    modeloEdicion.Marca = response.Data.Marca;
                    modeloEdicion.Precio = response.Data.Precio;
                    modeloEdicion.Kilometraje = response.Data.Kilometraje;
                    modeloEdicion.LitrosTanque = response.Data.LitrosTanque;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", "Error al conectar con el servidor");
            }
        }

        private async Task GuardarEdicion()
        {
            try
            {
                if (string.IsNullOrEmpty(matriculaEnEdicion)) return;

                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                // IMPORTANTE: Limpiar y reasignar para evitar errores de autorización
                Http.DefaultRequestHeaders.Authorization = null;
                Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Usamos matriculaEnEdicion en la URL
                var responseEditar = await Http.PutAsJsonAsync($"api/vehiculos/{matriculaEnEdicion}", modeloEdicion);

                if (responseEditar.IsSuccessStatusCode)
                {
                    await JS.InvokeVoidAsync("alert", "Vehículo actualizado correctamente");
                    mostrarModalModificar = false; // Cerramos el modal
                    await CargarVehiculos(token!); // Recargamos la lista
                }
                else
                {
                    var errorBody = await responseEditar.Content.ReadAsStringAsync();
                    await JS.InvokeVoidAsync("alert", $"Error al editar: {errorBody}");
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", "Error de conexión: " + ex.Message);
            }
        }
    }
}
