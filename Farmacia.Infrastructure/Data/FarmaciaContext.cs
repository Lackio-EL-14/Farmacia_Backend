using Microsoft.EntityFrameworkCore;
using Farmacia.Core.Entities;


namespace Farmacia.Infrastructure.Data
{
    public class FarmaciaContext : DbContext
    {
        public FarmaciaContext()
        { 
        }

            public FarmaciaContext(DbContextOptions<FarmaciaContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Unidad> Unidades { get; set; }
        public DbSet<TipoProducto> TiposProducto { get; set; }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<LoteProducto> LotesProducto { get; set; }

        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetallesCompra { get; set; }

        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }

        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Caja> Cajas { get; set; }
        public DbSet<ArqueoCaja> ArqueosCaja { get; set; }

        public DbSet<Traspaso> Traspasos { get; set; }
        public DbSet<BajaProducto> BajasProducto { get; set; }
        public DbSet<MovimientoInventario> MovimientosInventario { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FarmaciaContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
