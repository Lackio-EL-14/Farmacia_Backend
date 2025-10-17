using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Infrastructure.DTOs
{
    public class CajaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Activa { get; set; } = true;
    }
}
