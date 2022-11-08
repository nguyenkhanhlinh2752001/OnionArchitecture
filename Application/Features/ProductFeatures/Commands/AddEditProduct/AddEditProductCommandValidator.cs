using Application.Common;
using FluentValidation;

namespace Application.Features.ProductFeatures.Commands.AddEditProduct
{
    public class AddEditProductCommandValidator : AbstractValidator<AddEditProductCommand>
    {
        public AddEditProductCommandValidator()
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