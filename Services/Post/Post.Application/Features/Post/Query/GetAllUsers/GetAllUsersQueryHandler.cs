
using AutoMapper;
using MediatR;
using Post.Application.Contracts;
using Post.Application.DTOs;
using Post.Domain.Entities;

namespace Post.Applictaion.Features.Posts.Query;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<GetUserDTO>>
{
  private readonly IPostRepository _postRepository;
  private readonly IMapper _mapper;

  public GetAllUsersQueryHandler(IPostRepository postRepository, IMapper mapper)
  {
    _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
  }

  public async Task<IEnumerable<GetUserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
  {
        return _mapper.Map<IEnumerable<GetUserDTO>>(await _postRepository.GetUsers());
  }
}