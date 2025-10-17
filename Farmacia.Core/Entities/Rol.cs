using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Entities
{
    public class Rol : BaseEntities
    {
        //public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>(); 

    }
}
