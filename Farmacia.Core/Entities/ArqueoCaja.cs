using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Entities
{
    public class ArqueoCaja : BaseEntities
    {
        //public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Ingresos { get; set; }
        public decimal Egresos { get; set; }
        public decimal Diferencia { get; set; }

        public int CajaId { get; set; }
        public Caja Caja { get; set; } = null!;
    }
}
