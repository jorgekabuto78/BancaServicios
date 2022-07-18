using System.ComponentModel.DataAnnotations;

namespace BancaServiciosApi.Models
{
    public abstract class Persona
    {
        //public int PersonaId { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 5)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(1, MinimumLength = 1)]
        public string Genero { get; set; } = string.Empty;

        [Required]
        [Range(0, 130)]
        public int Edad { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage ="La identiicación debe tener 10 caracteres")]
        public string Identificacion { get; set; } = string.Empty;

        [Required]
        [StringLength(300, MinimumLength = 5, ErrorMessage ="La dirección debe tener entre 5 y 300 caracteres")]
        public string Direccion { get; set; } = string.Empty;

        [StringLength(10)]
        public string Telefono { get; set; } = string.Empty;
    }
}
