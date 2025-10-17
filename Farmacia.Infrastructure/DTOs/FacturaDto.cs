using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Infrastructure.DTOs
{
    public class FacturaDto
    {
        public int Id { get; set; }
        public int NumeroFactura { get; set; }
        public string NumeroAutorizacion { get; set; } = null!;
        public DateTime FechaEmision { get; set; } = DateTime.Now;

        public int VentaId { get; set; }
    }
}
