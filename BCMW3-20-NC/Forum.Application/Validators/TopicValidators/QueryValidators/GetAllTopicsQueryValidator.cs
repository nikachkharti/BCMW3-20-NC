using FluentValidation;
using Forum.Application.Features.Topics.Queries;

namespace Forum.Application.Validators.TopicValidators.QueryValidators
{
    public class GetAllTopicsQueryValidator : AbstractValidator<GetAllTopicsQuery>
    {
        public GetAllTopicsQueryValidator()
        {
            RuleFor(x => x.pageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.pageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0.");
        }
    }
}
