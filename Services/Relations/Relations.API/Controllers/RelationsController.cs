using Relations.API.Entities;
using Relations.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Relations.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RelationsController : ControllerBase
    {
        readonly IRelationsRepository _repository;
        public RelationsController(IRelationsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("friends/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetUserFriends(string userId)
        {
            var friends = await _repository.GetUserFriends(userId);
            return Ok(friends);
        }


    }

}
