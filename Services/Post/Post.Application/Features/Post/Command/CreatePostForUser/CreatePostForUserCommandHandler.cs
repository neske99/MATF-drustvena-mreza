using AutoMapper;
using MediatR;
using Post.Application.Contracts;
using Post.Application.DTOs;
using Post.Domain.Entities;

namespace Post.Application.Features.Posts.Command.CreatePostForUser;

public class CreatePostForUserCommandHandler : IRequestHandler<CreatePostForUserCommand,bool>
{
  private readonly IPostRepository _postRepository;
  private readonly IMapper _mapper;

  public CreatePostForUserCommandHandler(IPostRepository postRepository, IMapper mapper)
  {
    _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
  }

  Task<bool> IRequestHandler<CreatePostForUserCommand, bool>.Handle(CreatePostForUserCommand request, CancellationToken cancellationToken)
  {
    return _postRepository.CreatePostForUser(request.UserId, _mapper.Map<Post.Domain.Entities.Post>(request.Post));
  }
}