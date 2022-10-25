using Application.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Commands.CreateOrderCommand
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        private readonly ApplicationDbContext _context;

        public CreateOrderCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(p => p.ProductId)
                .NotNull().WithMessage("{PropertyName").OverridePropertyName("Product").WithMessage(ValidatorMessage.NotNullValidator)
                .NotEmpty().WithMessage("{PropertyName}").OverridePropertyName("Product").WithMessage(ValidatorMessage.NotEmptyValidator);

            RuleFor(p => p.Quantity)
                .NotNull().WithMessage("{PropertyName").OverridePropertyName("Quantity").WithMessage(ValidatorMessage.NotNullValidator)
                .NotEmpty().WithMessage("{PropertyName}").OverridePropertyName("Quantity").WithMessage(ValidatorMessage.NotEmptyValidator)
                .MustAsync(CurrentProductQuantity).WithMessage("{PropertyName}").OverridePropertyName("Quantity").WithMessage(ValidatorMessage.LessThanOrEqualValidator);
        }

        private async Task<bool> CurrentProductQuantity(CreateOrderCommand c,int id, CancellationToken token)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == c.ProductId);
            return !(product?.Quantity < c.Quantity);
        }
    }
}