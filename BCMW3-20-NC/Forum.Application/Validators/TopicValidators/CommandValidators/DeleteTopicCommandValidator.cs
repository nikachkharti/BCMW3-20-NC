using FluentValidation;
using Forum.Application.Features.Topics.Commands;

namespace Forum.Application.Validators.TopicValidators.CommandValidators
{
    public class DeleteTopicCommandValidator : AbstractValidator<DeleteTopicCommand>
    {
        public DeleteTopicCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}
