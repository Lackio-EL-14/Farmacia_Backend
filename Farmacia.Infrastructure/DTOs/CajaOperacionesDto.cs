using System.ComponentModel.DataAnnotations;

namespace Farmacia.Infrastructure.DTOs
{
    public class AperturaCajaDto
    {
        [Required]
        public int CajaId { get; set; }
    }

    public class CierreCajaDto
    {
        [Required]
        public int CajaId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El monto no puede ser negativo")]
        public decimal DineroFisicoEnCaja { get; set; }

        public decimal Egresos { get; set; } = 0;
    }
}