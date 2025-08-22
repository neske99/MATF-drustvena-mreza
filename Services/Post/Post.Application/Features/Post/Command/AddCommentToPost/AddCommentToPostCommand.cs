
using MediatR;
using Post.Application.DTOs;

namespace Post.Applictaion.Fetaures.Posts.Command;

public class AddCommentToPostCommand:IRequest<bool>
{
  public CreateCommentDTO Comment{ get; set; }
  public int PostId{ get; set; }

}