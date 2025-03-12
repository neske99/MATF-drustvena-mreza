using Relations.API.Entities;

namespace Relations.API.Repositories
{
    public interface IRelationsRepository
    {
        Task<IEnumerable<User>> GetUserFriends(string userId);

        Task<IEnumerable<User>> GetBlockedUsers(string userId);

        Task<IEnumerable<User>> GetSentFriendRequests(string userId);

        Task<IEnumerable<User>> GetReceivedFriendRequests(string userId);
    }
}
