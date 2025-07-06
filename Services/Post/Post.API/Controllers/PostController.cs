using Microsoft.AspNetCore.Mvc;
using Post.Application.Contracts;
using Post.Domain.Entities;
using Post.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Post.API.GrpcServices;

namespace Post.API.Controllers;

[ApiController]
[Route("[controller]")]
//[Authorize(Roles = "Buyer")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly GreeterService _greeterService;

    public PostController(IPostRepository repository, GreeterService greeterService)
    {
        this._postRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        this._greeterService = greeterService ?? throw new ArgumentException(nameof(greeterService));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> GetPostsForUser([FromQuery] int userId)
    {
        return Ok(await _postRepository.GetPostsForUser(userId));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> CreatePostForUser([FromQuery] int userId, [FromBody] Post.Domain.Entities.Post post)
    {
        return Ok(await _postRepository.CreatePostForUser(userId, post));
    }


    [HttpPost("[action]")]
    public async Task<ActionResult> AddCommentToPost([FromQuery] int postId, [FromBody] Comment comment)
    {
        return Ok(await _postRepository.AddCommentToPost(postId, comment));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> GetAllPosts()
    {
        return Ok(await _postRepository.GetAllAsync());
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> TestGrpc()
    {
        return Ok(_greeterService.SayHello());
    }

}
