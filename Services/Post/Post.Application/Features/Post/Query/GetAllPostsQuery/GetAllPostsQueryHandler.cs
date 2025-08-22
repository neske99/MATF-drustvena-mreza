
using AutoMapper;
using MediatR;
using Post.Application.Contracts;
using Post.Application.DTOs;

namespace Post.Application.Features.Posts.Query;

public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, IEnumerable<GetPostDTO>>
{

  private readonly IMapper _mapper;
  private readonly IPostRepository _postRepository;

  public GetAllPostsQueryHandler(IMapper mapper, IPostRepository postRepository)
  {
    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
  }
  public async Task<IEnumerable<GetPostDTO>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
  {
    return _mapper.Map<IEnumerable<GetPostDTO>>(await _postRepository.GetAllPostsAsync());
  }
}