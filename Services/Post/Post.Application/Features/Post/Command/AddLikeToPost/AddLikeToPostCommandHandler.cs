
using AutoMapper;
using MediatR;
using Post.Application.Contracts;
using Post.Application.DTOs;
using Post.Domain.Entities;

namespace Post.Applictaion.Fetaures.Posts.Command;

public class AddLikeToPostCommandHandler : IRequestHandler<AddLikeToPostCommand, bool>
{
  private readonly IPostRepository _postRepository;
  private readonly IMapper _mapper;

  public AddLikeToPostCommandHandler(IMapper mapper, IPostRepository postRepository)
  {
    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
  }

  public Task<bool> Handle(AddLikeToPostCommand request, CancellationToken cancellationToken)
  {
    return _postRepository.AddLikeToPost(request.PostId, _mapper.Map<Like>(request.Like));
  }
}