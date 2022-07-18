using System.ComponentModel.DataAnnotations;

namespace BancaServiciosApi.Dtos
{
    public class MovimientoDTO
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
     
        public DateTime FechaMovimiento { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 6)]
   
        public string TipoMovimiento { get; set; }

        [Required]
        public double Valor { get; set; }
            
    }
}
