using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Farmacia.Core.Entities;
using Farmacia.Infrastructure.DTOs;

namespace Farmacia.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ArqueoCaja, ArqueoCajaDto>().ReverseMap();
            CreateMap<BajaProducto, BajaProductoDto>().ReverseMap();
            CreateMap<Caja, CajaDto>().ReverseMap();
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Compra, CompraDto>().ReverseMap();
            CreateMap<DetalleCompra, DetalleCompraDto>().ReverseMap();
            CreateMap<DetalleVenta, DetalleVentaDto>().ReverseMap();
            CreateMap<Empresa, EmpresaDto>().ReverseMap();
            CreateMap<Factura, FacturaDtos>().ReverseMap();
            CreateMap<LoteProducto, LoteProductoDto>().ReverseMap();
            CreateMap<Marca, MarcaDto>().ReverseMap();
            CreateMap<MovimientoInventario, MovimientoInventarioDto>().ReverseMap();
            CreateMap<Proveedor, ProveedorDto>().ReverseMap();
            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<Rol, RolDto>().ReverseMap();
            CreateMap<Sucursal, SucursalDto>().ReverseMap();
            CreateMap<Traspaso, TraspasoDto>().ReverseMap();
            CreateMap<TipoProducto, TipoProductoDto>().ReverseMap();
            CreateMap<Unidad, UnidadDto>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Vendedor, VendedorDto>().ReverseMap();
            CreateMap<Venta, VentaDtos>().ReverseMap();



        }
    }
}
