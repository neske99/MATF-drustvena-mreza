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
        return Ok(_mapper.Map<IEnumerable<GetPostDTO>>(await _postRepository.GetPostsForUser(userId,_relationsService.GetFriends(userId))));
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


    /*[HttpGet("[action]")]
    public async Task<ActionResult> TestGrpc()
    {

        return Ok();
    }*/

}
