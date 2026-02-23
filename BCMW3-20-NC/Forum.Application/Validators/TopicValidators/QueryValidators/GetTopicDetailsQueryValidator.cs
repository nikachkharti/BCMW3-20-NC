using FluentValidation;
using Forum.Application.Features.Topics.Queries;

namespace Forum.Application.Validators.TopicValidators.QueryValidators
{
    public class GetTopicDetailsQueryValidator : AbstractValidator<GetTopicDetailsQuery>
    {
        public GetTopicDetailsQueryValidator()
        {
            RuleFor(x => x.topcId)
                .NotEmpty()
                .WithMessage("Topic id parameter is required");
        }
    }
}
