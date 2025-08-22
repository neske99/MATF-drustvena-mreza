using MediatR;
using Post.Application.DTOs;

namespace Post.Application.Features.Posts.Command.CreatePostForUser;
public class CreatePostForUserCommand: IRequest<bool>
{
    public int UserId { get; set; }
    public CreatePostDTO Post { get; set; } = new CreatePostDTO();
}