using Relations.API.Entities;

namespace Relations.API.Repositories
{
    public interface IRelationsRepository
    {
        Task<IEnumerable<User>> GetUsers();

        Task<User> CreateUser(User user);

        Task<Boolean> DeleteUser(string userId);

        Task<IEnumerable<User>> GetUserFriends(string userId);

        Task<Boolean> RemoveFriendship(string sourceUserId, string targetUserId);

        Task<IEnumerable<User>> GetBlockedUsers(string userId);

        Task<Boolean> BlockUser(string sourceUserId, string targetUserId);

        Task<Boolean> UnblockUser(string sourceUserId, string targetUserId);

        Task<IEnumerable<User>> GetSentFriendRequests(string userId);

        Task<Boolean> SendFriendRequest(string sourceUserId, string targetUserId);

        Task<Boolean> UnsendFriendRequest(string sourceUserId, string targetUserId);

        Task<IEnumerable<User>> GetReceivedFriendRequests(string userId);

        Task<Boolean> AcceptFriendRequest(string sourceUserId, string targetUserId);

        Task<Boolean> DeclineFriendRequest(string sourceUserId, string targetUserId);
    }
}
