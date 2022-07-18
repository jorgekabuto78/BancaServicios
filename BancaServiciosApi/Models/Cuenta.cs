using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancaServiciosApi.Models
{
    public class Cuenta
    {
        public int CuentaId { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4)]
        [Column("Numero")]
        public string NumeroCuenta { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 7)]
        [Column("Tipo")]
        public string TipoCuenta { get; set; }

        [Required]
        public double SaldoInicial { get; set; }

        [Required]
        public bool Estado { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
    }
}
