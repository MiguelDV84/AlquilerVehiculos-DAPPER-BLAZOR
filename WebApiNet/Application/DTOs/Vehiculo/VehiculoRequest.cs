using System.ComponentModel.DataAnnotations;
using WebApiNet.Core.Enums;

namespace WebApiNet.Application.DTOs.Vehiculo
{
    public class VehiculoRequest
    {
        [RegularExpression(@"^[0-9]{4}[BCDFGHJKLMNPQRSTVWXYZ]{3}$", ErrorMessage = "La matrícula debe tener 4 números y 3 consonantes válidas (ej. 1234BBB).")]
        public string Matricula { get; set; }

        [Required]
        public TipoVehiculo TipoVehiculo { get; set; }

        public int Kilometraje { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Los litros del tanque son obligatorios")]
        public double LitrosTanque { get; set; }
    }
}
