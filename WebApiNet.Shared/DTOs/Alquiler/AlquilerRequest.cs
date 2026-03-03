using System.ComponentModel.DataAnnotations;

namespace WebApiNet.Shared.DTOs.Alquiler
{
    public class AlquilerRequest : IValidatableObject
    {
        public DateOnly FechaDevolucionPrevista { get; set; }
        public string VehiculoMatricula { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(FechaDevolucionPrevista < DateOnly.FromDateTime(DateTime.Now))
            {
                yield return new ValidationResult("La fecha de devolución no puede ser anterior a la fecha actual.",
                    [nameof(FechaDevolucionPrevista)]);
            }
        }
    }
}
