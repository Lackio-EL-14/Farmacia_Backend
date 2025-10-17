using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Infrastructure.DTOs
{
    public class LoteProductoDto
    {
        public int Id { get; set; }
        public string NumeroLote { get; set; } = null!;
        public DateTime FechaFabricacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int Cantidad { get; set; }

        public int ProductoId { get; set; }
    }
}
