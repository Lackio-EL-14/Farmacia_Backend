using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Entities
{
    public class Cliente : BaseEntities
    {
        //public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? NIT { get; set; }
        public string? Telefono { get; set; }

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
