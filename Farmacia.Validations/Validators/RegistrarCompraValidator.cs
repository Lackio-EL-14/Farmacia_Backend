using Farmacia.Infrastructure.DTOs;
using FluentValidation;

namespace Farmacia.Validations.Validators
{
    public class RegistrarCompraValidator : AbstractValidator<RegistrarCompraDto>
    {
        public RegistrarCompraValidator()
        {
            RuleFor(x => x.ProveedorId)
                .GreaterThan(0).WithMessage("El proveedor es obligatorio.");

            RuleFor(x => x.Detalles)
                .NotEmpty().WithMessage("La compra debe tener al menos 1 detalle.");

            RuleForEach(x => x.Detalles).ChildRules(det =>
            {
                det.RuleFor(d => d.ProductoId)
                    .GreaterThan(0).WithMessage("ProductoId inválido.");

                det.RuleFor(d => d.Cantidad)
                    .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero.");

                det.RuleFor(d => d.PrecioUnitario)
                    .GreaterThan(0).WithMessage("El precio unitario debe ser mayor a cero.");
            });
        }
    }
}
