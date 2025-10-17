using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Infrastructure.DTOs
{
    public class VendedorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Comision { get; set; }
    }
}
