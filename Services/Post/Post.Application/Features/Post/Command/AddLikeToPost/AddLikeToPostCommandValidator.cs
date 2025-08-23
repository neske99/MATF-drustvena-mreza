

using FluentValidation;
using FluentValidation.Results;
using Post.Applictaion.Fetaures.Posts.Command;
using Post.Domain.Entities;

public class AddLikeToPostCommandValidator : AbstractValidator<AddLikeToPostCommand>
{
  public AddLikeToPostCommandValidator()
  {
    RuleFor(like => like.PostId).NotNull().Must(id => id > 0).WithMessage("PostId must be valid");
    RuleFor(like => like.Like).NotNull().WithMessage("Like must not be null");
    RuleFor(like => like.Like.UserId).NotNull().Must(id => id > 0).WithMessage("UserId must be valid");
  }
}