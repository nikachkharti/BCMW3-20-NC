using FluentValidation;
using Forum.Application.Features.Topics.Commands;

namespace Forum.Application.Validators.TopicValidators.CommandValidators
{
    public class UpdateTopicCommandValidator : AbstractValidator<UpdateTopicCommand>
    {
        public UpdateTopicCommandValidator()
        {
            RuleFor(x => x.model.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.model.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(50).WithMessage("Title must not exceed 50 characters.");

            RuleFor(x => x.model.Content)
                .NotEmpty().WithMessage("Content is required.");

            RuleFor(x => x.model.CommentsAreAllowed)
                .NotEmpty().WithMessage("CommentsAreAllowed is required.");
        }
    }
}
