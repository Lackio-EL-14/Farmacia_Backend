using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Entities
{
    public class BajaProducto : BaseEntities
    {
        //public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Motivo { get; set; } = null!;
        public int Cantidad { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;
    }
}
