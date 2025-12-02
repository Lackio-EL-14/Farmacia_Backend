using Farmacia.Core.Entities;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.DTOs;
using Farmacia.Core.Exceptions;

namespace Farmacia.Application.Services
{
    public class CajaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CajaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Caso de Uso: Habilitar Caja para ventas
        public async Task AbrirCajaAsync(AperturaCajaDto dto)
        {
            var caja = await _unitOfWork.Cajas.GetById(dto.CajaId);
            if (caja == null) throw new BusinessException("La caja no existe.");

            if (caja.Activa) throw new BusinessException("La caja ya está activa.");

            caja.Activa = true;
            await _unitOfWork.Cajas.Update(caja);
            await _unitOfWork.SaveChangesAsync();
        }

        // Caso de Uso: Cierre y Arqueo
        public async Task<ArqueoCaja> CerrarCajaAsync(CierreCajaDto dto)
        {
            var caja = await _unitOfWork.Cajas.GetById(dto.CajaId);
            if (caja == null) throw new BusinessException("La caja no existe.");

            if (!caja.Activa) throw new BusinessException("La caja ya está cerrada/inactiva.");

            // 1. Calcular Ventas del Día (Ingresos del Sistema)
            // Como tu entidad Arqueo no tiene "FechaApertura", sumamos las ventas de HOY.
            var fechaInicio = DateTime.Today; // 00:00:00 de hoy
            var fechaFin = DateTime.Now;

            var ventas = await _unitOfWork.Ventas.GetAll();

            // Asumimos que las ventas de esta caja son las del día. 
            // Nota: Si tuvieras varias cajas físicas, necesitaríamos filtrar por UsuarioId o CajaId en Venta.
            // Por ahora, sumamos todas las ventas del día (modelo simple).
            var ingresosSistema = ventas
                .Where(v => v.FechaVenta >= fechaInicio && v.FechaVenta <= fechaFin)
                .Sum(v => v.Total);

            // 2. Calcular Diferencia
            // Diferencia = Lo que tienes Físico - (Lo que vendiste - Gastos)
            var esperadoEnCaja = ingresosSistema - dto.Egresos;
            var diferencia = dto.DineroFisicoEnCaja - esperadoEnCaja;

            // 3. Crear Registro de Arqueo
            var arqueo = new ArqueoCaja
            {
                CajaId = dto.CajaId,
                Fecha = DateTime.Now,
                Ingresos = ingresosSistema,
                Egresos = dto.Egresos,
                Diferencia = diferencia
            };

            await _unitOfWork.Arqueos.Add(arqueo);

            // 4. Cerrar la Caja
            caja.Activa = false;
            await _unitOfWork.Cajas.Update(caja);

            await _unitOfWork.SaveChangesAsync();

            return arqueo;
        }
    }
}