using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Entities
{
    public class Caja : BaseEntities
    {
        //public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Activa { get; set; } = true;

        public ICollection<ArqueoCaja> Arqueos { get; set; } = new List<ArqueoCaja>();
    }
}
