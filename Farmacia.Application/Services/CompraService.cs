using Farmacia.Core.Entities;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.DTOs;
using Farmacia.Core.Exceptions;
using Farmacia.Core.Pagination;
using Farmacia.Core.Response; 

namespace Farmacia.Application.Services
{
    public class CompraService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompraService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedList<Compra>> GetAllComprasAsync(int pageNumber, int pageSize)
        {

            var compras = await _unitOfWork.Compras.GetAll();

            var pagedCompras = PagedList<Compra>.Create(compras, pageNumber, pageSize);

            return pagedCompras;
        }

        public async Task<int> RegistrarCompraAsync(RegistrarCompraDto dto)
        {
            await _unitOfWork.BeginTransaccionAsync(); // Iniciar transacción

            try
            {
                var proveedor = await _unitOfWork.Proveedores.GetById(dto.ProveedorId);
                if (proveedor == null)
                    throw new BusinessException("El proveedor especificado no existe.");

                var compra = new Compra
                {
                    Fecha = DateTime.Now,
                    Total = 0,
                    ProveedorId = dto.ProveedorId
                };

                await _unitOfWork.Compras.Add(compra);
                await _unitOfWork.SaveChangesAsync(); // Guardar cabecera para tener ID

                decimal totalAcumulado = 0;

                foreach (var det in dto.Detalles)
                {
                    var producto = await _unitOfWork.Productos.GetById(det.ProductoId);
                    if (producto == null)
                        throw new BusinessException($"El producto ID {det.ProductoId} no existe.");

                    decimal subtotal = det.Cantidad * det.PrecioUnitario;
                    totalAcumulado += subtotal;

                    var detalle = new DetalleCompra
                    {
                        CompraId = compra.Id,
                        ProductoId = producto.Id,
                        Cantidad = det.Cantidad,
                        PrecioUnitario = det.PrecioUnitario,
                        Subtotal = subtotal
                    };

                    await _unitOfWork.DetallesCompra.Add(detalle);

                    producto.StockTotal += det.Cantidad;
                    producto.PrecioCompra = det.PrecioUnitario;
                    await _unitOfWork.Productos.Update(producto);

                    var movimiento = new MovimientoInventario
                    {
                        ProductoId = producto.Id,
                        Fecha = DateTime.Now,
                        Tipo = "Entrada", // Usamos un string fijo para identificar que sumó stock
                        Cantidad = det.Cantidad
                        
                    };

                    await _unitOfWork.Movimientos.Add(movimiento);
                }

                compra.Total = totalAcumulado;
                await _unitOfWork.Compras.Update(compra);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return compra.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(); 
                throw;
            }
        }
    }
}