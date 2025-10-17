using Farmacia.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Infrastructure.Data.Configurations
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.ToTable("Productos");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.PrecioVenta)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PrecioCompra)
                .HasColumnType("decimal(18,2)");

           
            builder.HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId);

            builder.HasOne(p => p.Marca)
                .WithMany(m => m.Productos)
                .HasForeignKey(p => p.MarcaId);

            builder.HasOne(p => p.Unidad)
                .WithMany(u => u.Productos)
                .HasForeignKey(p => p.UnidadId);

            builder.HasOne(p => p.TipoProducto)
                .WithMany(t => t.Productos)
                .HasForeignKey(p => p.TipoProductoId);
        }
    }
}
