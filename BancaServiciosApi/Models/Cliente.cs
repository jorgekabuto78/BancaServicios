using System.ComponentModel.DataAnnotations;

namespace BancaServiciosApi.Models
{
    public class Cliente : Persona
    {
        public int ClienteId { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 5)]
        public string Contrasena { get; set; } = string.Empty;

        [Required]
        public bool Estado { get; set; } = true;

        public virtual ICollection<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
    }

}
