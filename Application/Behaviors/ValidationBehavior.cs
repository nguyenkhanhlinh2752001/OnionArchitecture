using FluentValidation;
using MediatR;

namespace Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResonse> : IPipelineBehavior<TRequest, TResonse>
        where TRequest : IRequest<TResonse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResonse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResonse> next)
        {
            if (_validators.Any())
            {
                var context = new FluentValidation.ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                    throw new Exceptions.ValidationException(failures);
            }
            return await next();
        }
    }
}