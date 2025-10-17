using AutoMapper;
using Farmacia.Core.Entities;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.DTOs;

namespace Farmacia.Application.Services
{
    public class VentaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VentaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Venta> RegistrarVenta(VentaDto dto)
        {
       
            if (dto.Detalles == null || !dto.Detalles.Any())
                throw new Exception("La venta debe contener al menos un producto.");

            var cliente = await _unitOfWork.Clientes.GetById(dto.ClienteId);
            if (cliente == null)
                throw new Exception("El cliente especificado no existe.");

    
            var venta = new Venta
            {
                ClienteId = dto.ClienteId,
                FechaHora = DateTime.Now,
                Total = 0
            };


            foreach (var det in dto.Detalles)
            {
                var producto = await _unitOfWork.Productos.GetById(det.ProductoId);
                if (producto == null)
                    throw new Exception($"El producto con ID {det.ProductoId} no existe.");


                if (producto.StockTotal < det.Cantidad)
                    throw new Exception($"Stock insuficiente para el producto: {producto.Nombre} (Stock actual: {producto.StockTotal})");

                producto.StockTotal -= det.Cantidad;

                var detalle = new DetalleVenta
                {
                    ProductoId = producto.Id,
                    Cantidad = det.Cantidad,
                    PrecioUnitario = det.PrecioUnitario,
                    Subtotal = det.Cantidad * det.PrecioUnitario
                };

                venta.Total += detalle.Subtotal;
                venta.Detalles.Add(detalle);
            }

            await _unitOfWork.Ventas.Add(venta);

            var factura = new Factura
            {
                NumeroFactura = new Random().Next(1000, 9999),
                NumeroAutorizacion = Guid.NewGuid().ToString().Substring(0, 10).ToUpper(),
                FechaEmision = DateTime.Now,
                Venta = venta
            };

            await _unitOfWork.Facturas.Add(factura);

            await _unitOfWork.SaveChangesAsync();

            return venta;
        }

        public async Task AnularVenta(int ventaId)
        {
            var venta = await _unitOfWork.Ventas.GetById(ventaId);
            if (venta == null)
                throw new Exception("La venta no existe.");

            foreach (var detalle in venta.Detalles)
            {
                var producto = await _unitOfWork.Productos.GetById(detalle.ProductoId);
                if (producto != null)
                {
                    producto.StockTotal += detalle.Cantidad;
                    await _unitOfWork.Productos.Update(producto);
                }
            }

            var factura = (await _unitOfWork.Facturas.GetAll())
                .FirstOrDefault(f => f.VentaId == venta.Id);

            if (factura != null)
            {
                factura.NumeroAutorizacion += "-ANULADA";
                await _unitOfWork.Facturas.Update(factura);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
