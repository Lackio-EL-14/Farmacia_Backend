using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Entities
{
    public class Proveedor : BaseEntities
    {
        //public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Contacto { get; set; }
        public string? Telefono { get; set; }

        public ICollection<Compra> Compras { get; set; } = new List<Compra>();
    }
}
