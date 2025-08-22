using Microsoft.AspNetCore.Mvc;
using Post.Application.Contracts;
using Post.Domain.Entities;
using Post.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Post.Application.DTOs;
using MediatR;
using Post.Application.Features.Posts.Command.CreatePostForUser;
using Post.Application.Features.Posts.Query;
using Post.Applictaion.Features.Posts.Query;
using Post.Applictaion.Fetaures.Posts.Command;

namespace Post.API.Controllers;

[ApiController]
[Route("[controller]")]
//[Authorize(Roles = "Buyer")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    private readonly IMediator _mediator;

  public PostController(IPostRepository repository, IMapper mapper, IMediator mediator)
  {
    this._postRepository = repository ?? throw new ArgumentNullException(nameof(repository));
    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
  }

    [HttpGet("[action]")]
    public async Task<ActionResult> GetPostsForUser([FromQuery] int userId)
    {
        /*var userFriends= _relationsService.GetFriends(userId);

        Dictionary<int, string> userIdToRelationsDict=new Dictionary<int, string>();


       var result= _mapper.Map<IEnumerable<GetPostDTO>>(await _postRepository.GetPostsForUser(userId, userFriends ));

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
        var userRelations = _relationsService.GetRelationShips(userId, userIdList);
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

        return Ok(_mapper.Map<IEnumerable<GetPostDTO>>(result));*/
        return Ok(await _mediator.Send(new GetPostForUserQuery(){UserId=userId}));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> CreatePostForUser([FromQuery] int userId, [FromBody] CreatePostDTO post)
    {
        CreatePostForUserCommand createCommand = new CreatePostForUserCommand()
        {
            UserId = userId,
            Post = post
        };
        return Ok(await _mediator.Send(createCommand));
    }


    [HttpPost("[action]")]
    public async Task<ActionResult> AddCommentToPost([FromQuery] int postId, [FromBody] CreateCommentDTO comment)
    {
        return Ok(await _mediator.Send(new AddCommentToPostCommand(){PostId=postId,Comment=comment}));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> AddLikeToPost([FromQuery] int postId, [FromBody] CreateLikeDTO like)
    {
        return Ok(await _mediator.Send(new AddLikeToPostCommand(){PostId=postId,Like=like}));
    }

    [HttpDelete("[action]")]
    public async Task<ActionResult> RemoveLikeFromPost([FromQuery] int postId, [FromQuery] int userId)
    {
        return Ok(await _mediator.Send(new RemoveLikeFromPostCommand(){UserId=userId,PostId=postId}));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> GetAllPosts()
    {
        return Ok(await _mediator.Send(new GetAllPostsQuery()));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> GetAllUsers()
    {

        return Ok(await _mediator.Send(new GetAllUsersQuery()));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> GetPostsCreatedByUser([FromQuery] int userId)
    {

        return Ok( await _mediator.Send(new GetPostCreatedByUserQuery()
        {
            UserId=userId
        }));
    }

}
