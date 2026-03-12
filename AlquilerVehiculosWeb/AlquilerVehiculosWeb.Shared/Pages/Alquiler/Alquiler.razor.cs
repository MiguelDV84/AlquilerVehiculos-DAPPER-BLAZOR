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
    public partial class Alquiler
    {
        // Lista de alquileres que se mostrarán en la página.
        private List<AlquilerResponse>? Listadoalquileres;

        private string textoBusqueda = "";
        private
        // Switch booleano para controlar si el usuario tiene permiso de ver la página.
        Boolean isAuthorized = false;

        private List<AlquilerResponse>? AlquilerFiltrados =>
            string.IsNullOrWhiteSpace(textoBusqueda)
                ? Listadoalquileres
                : Listadoalquileres?.Where(a =>
                    a.VehiculoMatricula.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase)
                ).ToList();

        // --- CICLO DE VIDA: AL CARGAR LA PÁGINA ---
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var MiToken = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                if (!string.IsNullOrEmpty(MiToken))
                {
                    isAuthorized = true;
                    
                    await CargarAlquileres(MiToken);

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

        private void IrANuevoAlquiler()
        {
            Navigation.NavigateTo("/alquiler/nuevo/");
        }

        private void IrAEditarAlquiler(String Matricula)
        {
            Navigation.NavigateTo($"/alquiler/editar/{Matricula}");
        }

        private void IrADetallesAlquiler(String matricula)
        {
            Navigation.NavigateTo($"/alquiler/detalle/{matricula}");
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
    }
}
