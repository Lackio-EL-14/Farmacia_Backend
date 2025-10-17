using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Entities
{
    public class Usuario : BaseEntities
    {
        //public int Id { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool Activo { get; set; } = true;

        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;
    }
}
