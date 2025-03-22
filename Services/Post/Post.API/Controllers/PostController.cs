using Microsoft.AspNetCore.Mvc;
using Post.Application.Contracts;
using Post.Domain.Entities;
using Post.Infrastructure.Repositories;

namespace Post.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    public PostController(IPostRepository repository)
    {
            this._postRepository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult> GetPostsForUser([FromQuery] int userId){
        return Ok(await _postRepository.GetPostsForUser(userId));
    }
    
    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult> CreatePostForUser(int userId,Post.Domain.Entities.Post post){
        return Ok(await _postRepository.CreatePostForUser(userId,post));
    }
    
    
    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult> AddCommentToPost(int postId,Comment comment){
        return Ok(await _postRepository.AddCommentToPost(postId,comment));
    }
    
    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult> GetAllPosts(){
        return Ok(await _postRepository.GetAllAsync());
    }

}
