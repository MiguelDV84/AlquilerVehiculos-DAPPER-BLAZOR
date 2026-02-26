using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiNet.Models
{
    public class Alquiler
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "La fecha del alquiler es obligatoria")]
        [Column(TypeName = "date")]
        public DateOnly FechaAlquiler { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);


        [Required(ErrorMessage = "La fecha de devolución es obligatoria")]
        [Column(TypeName = "date")]
        public DateOnly FechaDevolucionPrevista { get; set; }

        [Column(TypeName = "date")]
        public DateOnly? FechaDevolucionReal { get; set; }


        [Required(ErrorMessage = "El precio es obligatorio")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El DNI del cliente es obligatorio")]
        public string ClienteDni { get; set; }


        [ForeignKey("ClienteDni")]
        public virtual Cliente Cliente { get; set; }

        [Required(ErrorMessage = "La matrícula del vehículo es obligatoria")]
        public string VehiculoMatricula { get; set; }


        [ForeignKey("VehiculoMatricula")]
        public virtual Vehiculos Vehiculo { get; set; }
    }
}
