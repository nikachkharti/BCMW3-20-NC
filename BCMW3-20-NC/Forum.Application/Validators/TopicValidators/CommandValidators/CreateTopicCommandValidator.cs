using FluentValidation;
using Forum.Application.Features.Topics.Commands;

namespace Forum.Application.Validators.TopicValidators.CommandValidators
{
    public class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
    {
        public CreateTopicCommandValidator()
        {
            RuleFor(x => x.model.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(50).WithMessage("Title must not exceed 50 characters.");

            RuleFor(x => x.model.Content)
                .NotEmpty().WithMessage("Content is required.");
        }
    }
}
