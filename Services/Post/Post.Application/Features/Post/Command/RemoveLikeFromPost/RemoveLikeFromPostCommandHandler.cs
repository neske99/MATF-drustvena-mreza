using AutoMapper;
using MediatR;
using Post.Application.Contracts;
using Post.Application.DTOs;
using Post.Domain.Entities;

namespace Post.Applictaion.Fetaures.Posts.Command;

public class RemoveLikeFromPostCommandHandler : IRequestHandler<RemoveLikeFromPostCommand, bool>
{
  private readonly IPostRepository _postRepository;
  private readonly IMapper _mapper;

  public RemoveLikeFromPostCommandHandler(IMapper mapper, IPostRepository postRepository)
  {
    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
  }

  public Task<bool> Handle(RemoveLikeFromPostCommand request, CancellationToken cancellationToken)
  {
    return _postRepository.RemoveLikeFromPost(request.PostId,request.UserId);
  }
}