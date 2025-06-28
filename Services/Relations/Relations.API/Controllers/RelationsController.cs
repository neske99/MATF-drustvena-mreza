using Relations.API.Entities;
using Relations.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

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

        [HttpGet("users")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var friends = await _repository.GetUsers();
            return Ok(friends);
        }

        [HttpPost("users")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            var result = await _repository.CreateUser(user);
            return Ok(result);
        }

        [HttpDelete("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> DeleteUser([FromBody] string userId)
        {
            var result = await _repository.DeleteUser(userId);
            if (!result)
                return NotFound(null);
            return Ok();
        }

        [HttpGet("friends/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetUserFriends(string userId)
        {
            var friends = await _repository.GetUserFriends(userId);
            return Ok(friends);
        }

        [HttpDelete("friends/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> RemoveFriendship(string sourceUserId, string targetUserId)
        {
            var result = await _repository.RemoveFriendship(sourceUserId, targetUserId);
            if (!result)
                return NotFound(null);
            return Ok();
        }

        [HttpGet("blocked/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetBlockedUsers(string userId)
        {
            var blockedUsers = await _repository.GetBlockedUsers(userId);
            return Ok(blockedUsers);
        }

        [HttpPut("blocked/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> BlockUser(string sourceUserId, string targetUserId)
        {
            var result = await _repository.BlockUser(sourceUserId, targetUserId);
            if (!result)
                return NotFound(null);
            return Ok();
        }

        [HttpDelete("blocked/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> UnblockUser(string sourceUserId, string targetUserId)
        {
            var result = await _repository.UnblockUser(sourceUserId, targetUserId);
            if (!result)
                return NotFound(null);
            return Ok();
        }


        [HttpGet("sent/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetSentFriendRequests(string userId)
        {
            var sentFriendRequests = await _repository.GetSentFriendRequests(userId);
            return Ok(sentFriendRequests);
        }

        [HttpPut("sent/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> SendFriendRequest(string sourceUserId, string targetUserId)
        {
            var result = await _repository.SendFriendRequest(sourceUserId, targetUserId);
            if (!result)
                return NotFound(null);
            return Ok();
        }

        [HttpDelete("sent/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> UnsendFriendRequest(string sourceUserId, string targetUserId)
        {
            var result = await _repository.UnsendFriendRequest(sourceUserId, targetUserId);
            if (!result)
                return NotFound(null);
            return Ok();
        }



        [HttpGet("received/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetReceivedFriendRequests(string userId)
        {
            var receivedFriendRequests = await _repository.GetReceivedFriendRequests(userId);
            return Ok(receivedFriendRequests);
        }

        [HttpPut("received/accept/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> AcceptFriendRequest(string sourceUserId, string targetUserId)
        {
            var result = await _repository.AcceptFriendRequest(sourceUserId, targetUserId);
            if (!result)
                return NotFound(null);
            return Ok();
        }

        [HttpDelete("received/decline/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> DeclineFriendRequest(string sourceUserId, string targetUserId)
        {
            var result = await _repository.DeclineFriendRequest(sourceUserId, targetUserId);
            if (!result)
                return NotFound(null);
            return Ok();
        }








    }

}
