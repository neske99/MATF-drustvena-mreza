using Relations.Common.Entities;
using Relations.Common.Repositories;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using EventBusMessages.Events;

namespace Relations.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RelationsController : ControllerBase
    {
        readonly IRelationsRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
    public RelationsController(IRelationsRepository repository, IPublishEndpoint publishEndpoint)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }

    [HttpGet("relations/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetRelation(int sourceUserId, int targetUserId)
        {
            var result = await _repository.GetRelation(sourceUserId, targetUserId);
            return Ok(result);
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
        public async Task<ActionResult<User>> DeleteUser([FromBody] int userId)
        {
            var result = await _repository.DeleteUser(userId);
            if (!result)
                return NotFound(null);
            return Ok();
        }

        [HttpGet("friends/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetUserFriends(int userId)
        {
            var friends = await _repository.GetUserFriends(userId);
            return Ok(friends);
        }

        [HttpDelete("friends/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> RemoveFriendship(int sourceUserId, int targetUserId)
        {
            var result = await _repository.RemoveFriendship(sourceUserId, targetUserId);
            if (!result)
                return NotFound(null);
            return Ok();
        }

        [HttpGet("blocked/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetBlockedUsers(int userId)
        {
            var blockedUsers = await _repository.GetBlockedUsers(userId);
            return Ok(blockedUsers);
        }

        [HttpPut("blocked/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> BlockUser(int sourceUserId, int targetUserId)
        {
            var result = await _repository.BlockUser(sourceUserId, targetUserId);
            if (!result)
                return NotFound(null);
            return Ok();
        }

        [HttpDelete("blocked/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> UnblockUser(int sourceUserId, int targetUserId)
        {
            var result = await _repository.UnblockUser(sourceUserId, targetUserId);
            if (!result)
                return NotFound(null);
            return Ok();
        }


        [HttpGet("sent/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetSentFriendRequests(int userId)
        {
            var sentFriendRequests = await _repository.GetSentFriendRequests(userId);
            return Ok(sentFriendRequests);
        }

        [HttpPut("sent/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> SendFriendRequest(int sourceUserId, int targetUserId)
        {
            var result = await _repository.SendFriendRequest(sourceUserId, targetUserId);
            if (!result)
                return NotFound(null);
            return Ok();
        }

        [HttpDelete("sent/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> UnsendFriendRequest(int sourceUserId, int targetUserId)
        {
            var result = await _repository.UnsendFriendRequest(sourceUserId, targetUserId);
            if (!result)
                return NotFound(null);
            return Ok();
        }



        [HttpGet("received/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetReceivedFriendRequests(int userId)
        {
            var receivedFriendRequests = await _repository.GetReceivedFriendRequests(userId);
            return Ok(receivedFriendRequests);
        }

        [HttpPut("received/accept/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> AcceptFriendRequest(int sourceUserId, int targetUserId)
        {
            var result = await _repository.AcceptFriendRequest(sourceUserId, targetUserId);

            if (!result)
                return NotFound(null);

            await _publishEndpoint.Publish(new RelationCreatedEvent
            {
                UserAId = sourceUserId,
                UserBId = targetUserId
            });
            return Ok();
        }

        [HttpDelete("received/decline/{sourceUserId}/{targetUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> DeclineFriendRequest(int sourceUserId, int targetUserId)
        {
            var result = await _repository.DeclineFriendRequest(sourceUserId, targetUserId);
            if (!result)
                return NotFound(null);
            return Ok();
        }


    }

}
