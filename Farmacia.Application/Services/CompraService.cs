using Farmacia.Core.Entities;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.DTOs;

namespace Farmacia.Application.Services
{
    public class CompraService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompraService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> RegistrarCompraAsync(RegistrarCompraDto dto)
        {
            var proveedor = await _unitOfWork.Proveedores.GetById(dto.ProveedorId);
            if (proveedor == null)
                throw new Exception("El proveedor no existe.");

            // Crear la compra
            var compra = new Compra
            {
                Fecha = DateTime.Now,
                Total = 0,
                ProveedorId = dto.ProveedorId
            };

            await _unitOfWork.Compras.Add(compra);

            decimal total = 0;

            foreach (var det in dto.Detalles)
            {
                var producto = await _unitOfWork.Productos.GetById(det.ProductoId);
                if (producto == null)
                    throw new Exception($"Producto {det.ProductoId} no existe.");

                // Calcular subtotal
                var subtotal = det.Cantidad * det.PrecioUnitario;
                total += subtotal;

                var detalle = new DetalleCompra
                {
                    CompraId = compra.Id,
                    ProductoId = producto.Id,
                    Cantidad = det.Cantidad,
                    PrecioUnitario = det.PrecioUnitario,
                    Subtotal = subtotal
                };

                await _unitOfWork.DetallesCompra.Add(detalle);

                // Actualizar stock del producto
                producto.StockTotal += det.Cantidad;
                await _unitOfWork.Productos.Update(producto);
            }

            compra.Total = total;

            await _unitOfWork.SaveChangesAsync();

            return compra.Id;
        }
    }
}
