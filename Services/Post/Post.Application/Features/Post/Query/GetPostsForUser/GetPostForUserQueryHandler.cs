using AutoMapper;
using MediatR;
using Post.Application.Contracts;
using Post.Application.DTOs;
using Post.Infrastructure.Contracts;

namespace Post.Application.Features.Posts.Query;

public class GetPostForUserQueryHandler : IRequestHandler<GetPostForUserQuery,IEnumerable<GetPostDTO>>
{
  private readonly IMapper _mapper;
  private readonly IPostRepository _postRepository;

  private readonly IRelationService _relationService;
  public GetPostForUserQueryHandler(IMapper mapper, IPostRepository postRepository, IRelationService relationService)
  {
    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
    _relationService = relationService ?? throw new ArgumentNullException(nameof(relationService));
  }

  public async Task<IEnumerable<GetPostDTO>> Handle(GetPostForUserQuery request, CancellationToken cancellationToken)
  {
        var userFriends= _relationService.GetFriends(request.UserId);

        Dictionary<int, string> userIdToRelationsDict=new Dictionary<int, string>();


       var result= _mapper.Map<IEnumerable<GetPostDTO>>(await _postRepository.GetPostsForUser(request.UserId, userFriends ));

       foreach (var post in result)
        {
            if (post.User != null)
            {
                if(!userIdToRelationsDict.ContainsKey(post.User.Id))
                    userIdToRelationsDict.Add(post.User.Id,"Nesto   je");

                foreach (var comment in post.Comments)
                {
                    if (comment.User != null )
                    {
                        if(!userIdToRelationsDict.ContainsKey(comment.User.Id))
                            userIdToRelationsDict.Add(comment.User.Id, "Nesto    je ");
                    }
                }

                // Add like users to the relations dictionary
                foreach (var like in post.Likes)
                {
                    if (like.User != null)
                    {
                        if (!userIdToRelationsDict.ContainsKey(like.User.Id))
                            userIdToRelationsDict.Add(like.User.Id, "Nesto je");
                    }
                }
            }

        }

        var userIdList = userIdToRelationsDict.Keys.ToList();
        var userRelations = _relationService.GetRelationShips(request.UserId, userIdList);
        for(int i=0;i<userIdList.Count;i++)
        {
            userIdToRelationsDict[userIdList[i]] = userRelations[i];
        }

       foreach (var post in result)
            {
                if (post.User != null)
                {
                    post.User.Relation = userIdToRelationsDict[post.User.Id];

                    foreach (var comment in post.Comments)
                    {
                        if (comment.User != null)
                        {
                            comment.User.Relation = userIdToRelationsDict[comment.User.Id];
                        }
                    }

                    // Set relations for like users
                    foreach (var like in post.Likes)
                    {
                        if (like.User != null)
                        {
                            like.User.Relation = userIdToRelationsDict[like.User.Id];
                        }
                    }
                }

            }

        return _mapper.Map<IEnumerable<GetPostDTO>>(result);
  }
}