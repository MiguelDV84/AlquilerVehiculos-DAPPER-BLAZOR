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

        private List<VehiculoResponse>? VehiculoFiltrados =>
            string.IsNullOrWhiteSpace(textoBusqueda)
                ? listaDeVehiculo
                : listaDeVehiculo?.Where(v =>
                    v.Marca.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                    v.Modelo.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase)
                  ).ToList();

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

        private void VerDetalles(string matricula) => Navigation.NavigateTo($"/vehiculo/{matricula}");
        private void EditarVehiculo(string matricula) => Navigation.NavigateTo($"/editar-vehiculo/{matricula}");
        private void IrANuevoVehiculo() => Navigation.NavigateTo("/nuevo-vehiculo");

        private async Task BorrarVehiculo(string matricula)
        {
            bool confirmado = await JS.InvokeAsync<bool>("confirm", $"¿Borrar vehículo {matricula}?");
            if (confirmado)
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                var request = new HttpRequestMessage(HttpMethod.Delete, $"api/vehiculos/{matricula}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    await CargarVehiculos(token!);
                }
                else
                {
                    await JS.InvokeVoidAsync("alert", "Error al borrar el vehículo.");
                }
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
    }
}
