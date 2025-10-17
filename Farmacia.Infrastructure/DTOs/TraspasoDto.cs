using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Infrastructure.DTOs
{
    public class TraspasoDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Enviado, Recibido

        public int ProductoId { get; set; }
    }
}
