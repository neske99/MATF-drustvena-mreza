
using MediatR;
using Post.Application.DTOs;

namespace Post.Application.Features.Posts.Query;

public class GetPostCreatedByUserQuery: IRequest<IEnumerable<GetPostDTO>>
{
  public int UserId { get; set; }

}