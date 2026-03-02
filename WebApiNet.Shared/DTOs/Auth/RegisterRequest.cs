using System.ComponentModel.DataAnnotations;

namespace WebApiNet.Application.DTOs.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(@"^[0-9]{8}[A-Z]$", ErrorMessage = "Formato de DNI no válido.")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
