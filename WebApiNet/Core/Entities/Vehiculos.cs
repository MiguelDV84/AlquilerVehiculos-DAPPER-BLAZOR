using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiNet.Shared.Enums;

namespace WebApiNet.Core.Entities
{
    public class Vehiculo
    {
        [Key]
        [RegularExpression(@"^[0-9]{4}[BCDFGHJKLMNPQRSTVWXYZ]{3}$", ErrorMessage = "La matrícula debe tener 4 números y 3 consonantes válidas (ej. 1234BBB).")]
        public string Matricula { get; set; }

        [Required(ErrorMessage = "El tipo de vehiculo es obligatorio")]
        public TipoVehiculo TipoVehiculo { get; set; }

        public int Kilometraje{ get; set; } = 0;

        [Required(ErrorMessage = "La marca es obligatoria")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio")]
        public string Modelo { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }

        public double LitrosTanque { get; set; }

        public EstadoVehiculo Estado { get; set; } = EstadoVehiculo.Disponible;

        public virtual ICollection<Alquiler> Alquileres { get; set; } = new List<Alquiler>();
    }
}
