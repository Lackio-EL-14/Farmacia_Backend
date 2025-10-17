using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Infrastructure.DTOs
{
    public class MovimientoInventarioDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Tipo { get; set; } = null!; // Entrada, Salida, Ajuste
        public int Cantidad { get; set; }

        public int ProductoId { get; set; }
    }
}
