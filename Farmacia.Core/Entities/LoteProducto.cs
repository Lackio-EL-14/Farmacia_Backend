using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Entities
{
    public class LoteProducto : BaseEntities
    {
        //public int Id { get; set; }
        public string NumeroLote { get; set; } = null!;
        public DateTime FechaFabricacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int Cantidad { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;
    }
}
