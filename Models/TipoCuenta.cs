using Presupuesto.Validations;
using System.ComponentModel.DataAnnotations;

namespace Presupuesto.Models
{
    public class TipoCuenta
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength:50, MinimumLength = 3, ErrorMessage = "La longitud del {0} campo debe estar entre {2} y {1}")]
        [Display(Name = "Nombre del tipo de cuenta")]
        [ValidarMayuscula]
        public string? Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get;}

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Nombre != null && Nombre.Length > 0)
            {
                var firtsLetter = Nombre[0].ToString();
                if(firtsLetter != firtsLetter.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula", new[] { nameof(Nombre) });
                }
            }
        }
    }
}
