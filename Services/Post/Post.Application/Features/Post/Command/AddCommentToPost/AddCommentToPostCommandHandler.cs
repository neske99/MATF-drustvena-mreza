
using AutoMapper;
using MediatR;
using Post.Application.Contracts;
using Post.Domain.Entities;

namespace Post.Applictaion.Fetaures.Posts.Command;

public class AddCommentToPostCommandHandler : IRequestHandler<AddCommentToPostCommand, bool>
{

  private readonly IPostRepository _postRepository;
  private readonly IMapper _mapper;

  public AddCommentToPostCommandHandler(IPostRepository postRepository, IMapper mapper)
  {
    _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
  }


  public Task<bool> Handle(AddCommentToPostCommand request, CancellationToken cancellationToken)
  {
    return _postRepository.AddCommentToPost(request.PostId, _mapper.Map<Comment>(request.Comment));
  }
}