

using FluentValidation;
using Post.Applictaion.Fetaures.Posts.Command;

public class AddCommentToPostCommandValidator: AbstractValidator<AddCommentToPostCommand>
{
  public AddCommentToPostCommandValidator()
  {
    RuleFor(comment => comment.PostId).NotNull().Must(x => x > 0).WithMessage("PostId must be valid") ;
    RuleFor(comment => comment.Comment.UserId).NotNull().Must(x => x > 0).WithMessage("UserId must be valid");
    RuleFor(comment => comment.Comment.Text).NotNull().NotEmpty().WithMessage("Comment text must be valid");

  }

}