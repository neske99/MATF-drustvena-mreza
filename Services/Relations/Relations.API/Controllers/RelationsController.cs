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

        [HttpGet("blocked/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetBlockedUsers(string userId)
        {
            var blockedUsers = await _repository.GetBlockedUsers(userId);
            return Ok(blockedUsers);
        }

        [HttpGet("sent/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetSentFriendRequests(string userId)
        {
            var sentFriendRequests = await _repository.GetSentFriendRequests(userId);
            return Ok(sentFriendRequests);
        }

        [HttpGet("received/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetReceivedFriendRequests(string userId)
        {
            var receivedFriendRequests = await _repository.GetReceivedFriendRequests(userId);
            return Ok(receivedFriendRequests);
        }






    }

}
