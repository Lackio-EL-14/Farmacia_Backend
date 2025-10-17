using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Entities
{
    public class Sucursal : BaseEntities
    {
        public string Nombre { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }


}
