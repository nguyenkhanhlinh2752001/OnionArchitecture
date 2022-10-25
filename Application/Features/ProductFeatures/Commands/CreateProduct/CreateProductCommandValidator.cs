using Application.Common;
using FluentValidation;

namespace Application.Features.ProductFeatures.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotNull().WithMessage("{PropertyName").OverridePropertyName("Name").WithMessage(ValidatorMessage.NotNullValidator)
                .NotEmpty().WithMessage("{PropertyName}").OverridePropertyName("Name").WithMessage(ValidatorMessage.NotEmptyValidator);
            RuleFor(p => p.Quantity)
                .NotNull().WithMessage("{PropertyName").OverridePropertyName("Name").WithMessage(ValidatorMessage.NotNullValidator)
                .NotEmpty().WithMessage("{PropertyName}").OverridePropertyName("Name").WithMessage(ValidatorMessage.NotEmptyValidator);
            RuleFor(p => p.Price)
                .NotNull().WithMessage("{PropertyName").OverridePropertyName("Name").WithMessage(ValidatorMessage.NotNullValidator)
                .NotEmpty().WithMessage("{PropertyName}").OverridePropertyName("Name").WithMessage(ValidatorMessage.NotEmptyValidator);
        }
    }
}