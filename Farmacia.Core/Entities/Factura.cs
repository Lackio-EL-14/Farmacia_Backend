using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Entities
{
    public class Factura : BaseEntities
    {
        //public int Id { get; set; }
        public int NumeroFactura { get; set; }
        public string NumeroAutorizacion { get; set; } = null!;
        public DateTime FechaEmision { get; set; } = DateTime.Now;

        public int VentaId { get; set; }
        public Venta Venta { get; set; } = null!;
    }
}
