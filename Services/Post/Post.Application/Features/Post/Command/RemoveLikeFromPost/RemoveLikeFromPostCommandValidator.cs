

using FluentValidation;
using Post.Applictaion.Fetaures.Posts.Command;

public class RemoveLikeFromPostCommandValidator : AbstractValidator<RemoveLikeFromPostCommand>
{

  public RemoveLikeFromPostCommandValidator()
  {
    RuleFor(x => x.UserId).NotNull().Must(id => id > 0).WithMessage("UserId must be valid");
    RuleFor(x => x.PostId).NotNull().Must(id => id > 0).WithMessage("PostId must be valid");

  }

}