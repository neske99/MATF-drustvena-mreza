

using FluentValidation;
using Post.Application.Features.Posts.Command.CreatePostForUser;

public class CreatePostForUserCommandValidator: AbstractValidator<CreatePostForUserCommand>
{
  public CreatePostForUserCommandValidator()
  {
    RuleFor(post => post.Post).NotNull().WithMessage("Post must not be null");
    RuleFor(post => post.Post.UserId).NotNull().Must(id => id > 0).WithMessage("User id must be valid");
    RuleFor(post => post.Post.Text).NotNull().NotEmpty().WithMessage("Post text can't be empty");
  }
}