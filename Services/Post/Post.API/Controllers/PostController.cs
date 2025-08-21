using Microsoft.AspNetCore.Mvc;
using Post.Application.Contracts;
using Post.Domain.Entities;
using Post.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Post.API.GrpcServices;
using AutoMapper;
using Post.Application.DTOs;

namespace Post.API.Controllers;

[ApiController]
[Route("[controller]")]
//[Authorize(Roles = "Buyer")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly RelationsService _relationsService;
    private readonly IMapper _mapper;

    public PostController(IPostRepository repository, RelationsService greeterService, IMapper mapper)
    {
        this._postRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        this._relationsService = greeterService ?? throw new ArgumentException(nameof(greeterService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> GetPostsForUser([FromQuery] int userId)
    {
        var userFriends= _relationsService.GetFriends(userId);

        Dictionary<int, string> userIdToRelationsDict=new Dictionary<int, string>();
/*        for(int i=0;i<userFriends.Count;i++)
        {
            userIdToRelationsDict.Add(userFriends[i], userRelations[i]);
        }*/

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

        return Ok(_mapper.Map<IEnumerable<GetPostDTO>>(result));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> CreatePostForUser([FromQuery] int userId, [FromBody] CreatePostDTO post)
    {
        return Ok(await _postRepository.CreatePostForUser(userId, _mapper.Map<Post.Domain.Entities.Post>(post)));
    }


    [HttpPost("[action]")]
    public async Task<ActionResult> AddCommentToPost([FromQuery] int postId, [FromBody] CreateCommentDTO comment)
    {
        return Ok(await _postRepository.AddCommentToPost(postId, _mapper.Map<Comment>( comment)));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> AddLikeToPost([FromQuery] int postId, [FromBody] CreateLikeDTO like)
    {
        return Ok(await _postRepository.AddLikeToPost(postId, _mapper.Map<Like>(like)));
    }

    [HttpDelete("[action]")]
    public async Task<ActionResult> RemoveLikeFromPost([FromQuery] int postId, [FromQuery] int userId)
    {
        return Ok(await _postRepository.RemoveLikeFromPost(postId, userId));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> GetAllPosts()
    {
        //return Ok(await _postRepository.GetAllPostsAsync());
        return Ok(_mapper.Map<IEnumerable<GetPostDTO>>(await _postRepository.GetAllPostsAsync()));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> GetUser()
    {

        return Ok(_mapper.Map<IEnumerable<GetUserDTO>>(await _postRepository.GetUsers()));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> GetPostsCreatedByUser([FromQuery] int userId)
    {

        return Ok(_mapper.Map<IEnumerable<GetPostDTO>>(await _postRepository.GetPostsCreatedByUser(userId)));
    }


    [HttpGet("[action]")]
    public async Task<ActionResult> TestGrpc([FromQuery] int userId,[FromQuery] List<int> userIdList)
    {
        return Ok(_relationsService.GetRelationShips(userId,userIdList));
    }

}
