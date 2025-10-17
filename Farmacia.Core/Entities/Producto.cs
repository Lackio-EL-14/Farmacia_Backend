using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Entities
{
    public class Producto : BaseEntities
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal PrecioCompra { get; set; }
        public int StockTotal { get; set; }

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;

        public int MarcaId { get; set; }
        public Marca Marca { get; set; } = null!;

        public int UnidadId { get; set; }
        public Unidad Unidad { get; set; } = null!;

        public int TipoProductoId { get; set; }
        public TipoProducto TipoProducto { get; set; } = null!;

       
        public int SucursalId { get; set; }
        public Sucursal Sucursal { get; set; } = null!;

        public ICollection<LoteProducto> Lotes { get; set; } = new List<LoteProducto>();
    }
}
