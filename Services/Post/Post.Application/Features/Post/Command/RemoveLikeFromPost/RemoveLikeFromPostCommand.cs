
using MediatR;
using Post.Application.DTOs;

namespace Post.Applictaion.Fetaures.Posts.Command;

public class RemoveLikeFromPostCommand:IRequest<bool>
{
    public int PostId { get; set; }
    public int UserId{ get; set; }
}