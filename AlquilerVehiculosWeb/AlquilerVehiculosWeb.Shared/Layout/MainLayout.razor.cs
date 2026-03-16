using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlquilerVehiculosWeb.Shared.Layout
{
    public partial class MainLayout
    {
        private string? usuarioNombre;
        private bool isNavMenuCollapsed = true;
        private bool isUserMenuOpen = false;

        private void ToggleNavMenu()
        {
            isNavMenuCollapsed = !isNavMenuCollapsed;
        }

        // --- SINCRONIZACIÓN DE ESTADO ---
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // 1. Buscamos el token primero
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                if (string.IsNullOrEmpty(token))
                {
                    // Si no hay token y no estamos en la página de login, mandamos al login UNA SOLA VEZ
                    var actualUri = Navigation.Uri;
                    if (!actualUri.Contains("/login"))
                    {
                        usuarioNombre = null;
                        Navigation.NavigateTo("/login");
                    }
                    return; // Cortamos la ejecución aquí
                }

                // 2. Si hay token, intentamos recuperar el nombre
                var nombre = await JS.InvokeAsync<string>("localStorage.getItem", "usuarioNombre");
                if (!string.IsNullOrEmpty(nombre))
                {
                    usuarioNombre = nombre;
                    StateHasChanged();
                }
            }
        }

        // --- ACCIÓN: LOGOUT ---
        private async Task CerrarSesion()
        {
            // 1. Limpiamos el almacenamiento local (borramos Token y Nombre).
            await JS.InvokeVoidAsync("localStorage.removeItem", "token");
            await JS.InvokeVoidAsync("localStorage.removeItem", "usuarioNombre");

            // 2. Limpiamos la variable local.
            usuarioNombre = null;

            // 3. Redirigimos al Login y forzamos recarga para limpiar cualquier rastro de la sesión.
            Navigation.NavigateTo("/login", forceLoad: true);
        }

        // Métodos para controlar el despliegue de menús (UX)
        private void ToggleUserMenu() => isUserMenuOpen = !isUserMenuOpen;
        private void CloseMenu() { isNavMenuCollapsed = true; isUserMenuOpen = false; }
    }
}
