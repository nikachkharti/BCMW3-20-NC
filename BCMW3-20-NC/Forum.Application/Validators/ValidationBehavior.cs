using FluentValidation;
using Forum.Application.Exceptions;
using MediatR;
using System.Collections.Immutable;

namespace Forum.Application.Validators
{
    public class ValidationBehavior<TRequest, TResponse>
        (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationFailures = await Task
                .WhenAll(validators
                    .Select(validator => validator
                        .ValidateAsync(context, cancellationToken)
                    )
                );

            var errors = validationFailures
                            .Where(validationResult => !validationResult.IsValid)
                            .SelectMany(validationResult => validationResult.Errors)
                            .GroupBy(validationFailures => validationFailures.PropertyName)
                            .ToImmutableDictionary(
                            pairs => pairs.Key,
                            pairs => pairs.Select(x => x.ErrorMessage).ToArray());

            if (errors.Any())
            {
                throw new BadRequestException(errors);
            }

            return await next();
        }
    }
}
