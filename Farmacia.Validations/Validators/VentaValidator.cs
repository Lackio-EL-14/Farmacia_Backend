using FluentValidation;
using Farmacia.Infrastructure.DTOs;


namespace Farmacia.Validations.Validators
{
    public class VentaValidator : AbstractValidator<VentaDto>
    {
        public VentaValidator()
        {
            RuleFor(v => v.ClienteId)
                .GreaterThan(0).WithMessage("Debe especificar un cliente válido.");

            RuleFor(v => v.Detalles)
                .NotEmpty().WithMessage("La venta debe incluir al menos un producto.");

            RuleForEach(v => v.Detalles).ChildRules(det =>
            {
                det.RuleFor(d => d.ProductoId)
                   .GreaterThan(0).WithMessage("Debe especificar un producto válido.");

                det.RuleFor(d => d.Cantidad)
                   .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero.");

                det.RuleFor(d => d.PrecioUnitario)
                   .GreaterThan(0).WithMessage("El precio unitario debe ser mayor a cero.");
            });
        }
    }
}
