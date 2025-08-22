
using MediatR;
using Post.Application.DTOs;

namespace Post.Application.Features.Posts.Query;

public class GetAllPostsQuery : IRequest<IEnumerable<GetPostDTO>>
{


}