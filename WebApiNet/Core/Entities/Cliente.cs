using System.ComponentModel.DataAnnotations;
using WebApiNet.Shared.Enums;

namespace WebApiNet.Core.Entities
{
    public class Cliente
    {
        [Key]
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(@"^[0-9]{8}[A-Z]$", ErrorMessage = "Formato de DNI no válido.")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        public string Nombre { get; set; }

        public Role Role { get; set; } = Role.User;

        public virtual ICollection<Alquiler> Alquileres { get; set; } = new List<Alquiler>();

    }
}
