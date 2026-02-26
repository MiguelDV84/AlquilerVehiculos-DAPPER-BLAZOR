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

        public DateOnly? FechaDevolucionReal { get; set; }


        [Required(ErrorMessage = "El precio es obligatorio")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }

        public string ClienteDni { get; set; }


        [Required(ErrorMessage = "El cliente es obligatorio")]
        [ForeignKey("ClienteDni")]
        public virtual Cliente Cliente { get; set; }

        public string VehiculoMatricula { get; set; }


        [Required(ErrorMessage = "La película es obligatoria")]
        [ForeignKey("VehiculoMatricula")]
        public virtual Vehiculos Vehiculo { get; set; }
    }
}
