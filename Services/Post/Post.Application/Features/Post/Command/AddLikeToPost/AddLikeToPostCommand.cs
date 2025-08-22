
using MediatR;
using Post.Application.DTOs;

namespace Post.Applictaion.Fetaures.Posts.Command;

public class AddLikeToPostCommand:IRequest<bool>
{
    public int PostId { get; set; }
    public CreateLikeDTO Like{ get; set; }
}