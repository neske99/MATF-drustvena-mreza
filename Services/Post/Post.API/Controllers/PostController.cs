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
        return Ok(await _mediator.Send(new GetPostForUserQuery(){UserId=userId}));
    }

    [HttpPost("[action]")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePostForUser(
    [FromForm] int userId,
    [FromForm] string text,
    [FromForm] IFormFile? file
)
    {
        string? fileUrl = null;
        string? fileName = null;
        string? fileType = null;

        if (file != null && file.Length > 0)
        {
            // Fixed: Add wwwroot to the path so static file middleware can serve the files
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "post-files");
            Directory.CreateDirectory(uploadsFolder);
            fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            fileUrl = $"/uploads/post-files/{fileName}";
            fileType = file.ContentType;
        }

        var post = new CreatePostDTO
        {
            UserId = userId,
            Text = text,
            FileUrl = fileUrl,
            FileName = fileName,
            FileType = fileType,
        };

        var createCommand = new CreatePostForUserCommand
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

    [HttpDelete("[action]")]
    public async Task<ActionResult> DeletePost([FromQuery] int postId, [FromQuery] int userId)
    {
        var result = await _postRepository.DeletePost(postId, userId);
        if (result)
        {
            return Ok(new { message = "Post deleted successfully" });
        }
        return NotFound(new { message = "Post not found or you don't have permission to delete it" });
    }
}
