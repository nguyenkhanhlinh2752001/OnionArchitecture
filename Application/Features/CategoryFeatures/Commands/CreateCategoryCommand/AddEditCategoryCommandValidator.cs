using Application.Common;
using FluentValidation;

namespace Application.Features.CategoryFeatures.Commands.CreateCategoryCommand
{
    public class AddEditCategoryCommandValidator : AbstractValidator<AddEditCategoryCommand>
    {
        public AddEditCategoryCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotNull().WithMessage("{PropertyName}").OverridePropertyName("Category name").WithMessage(ValidatorMessage.NotNullValidator)
                .NotEmpty().WithMessage("{ProperyName}").OverridePropertyName("Category name").WithMessage(ValidatorMessage.NotEmptyValidator);
        }
    }
}