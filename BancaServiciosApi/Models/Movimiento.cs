using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancaServiciosApi.Models
{
    public class Movimiento
    {
        public int MovimientoId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column("Fecha")]
        public DateTime FechaMovimiento { get; set; }

        [Required]
        [StringLength(8,MinimumLength = 6)]
        [Column("Tipo")]
        public string TipoMovimiento { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        public double Saldo { get; set; }

        public virtual Cuenta Cuenta { get; set; }

    }
}
