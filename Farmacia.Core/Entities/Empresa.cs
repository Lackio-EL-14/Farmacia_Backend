using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Entities
{
    public class Empresa : BaseEntities
    {
        //public int Id { get; set; }
        public string RazonSocial { get; set; } = null!;
        public string NIT { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Telefono { get; set; } = null!;

        public ICollection<Sucursal> Sucursales { get; set; } = new List<Sucursal>();
    }
}
