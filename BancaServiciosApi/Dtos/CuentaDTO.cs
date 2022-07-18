using System.ComponentModel.DataAnnotations;

namespace BancaServiciosApi.Dtos
{
    public class CuentaDTO
    {
        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string NumeroCuenta { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 7)]
        public string TipoCuenta { get; set; }

        [Required]
        public double SaldoInicial { get; set; }

    }
}
