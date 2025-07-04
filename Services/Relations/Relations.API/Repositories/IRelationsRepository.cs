﻿using Relations.API.Entities;

namespace Relations.API.Repositories
{
    public interface IRelationsRepository
    {
        Task<IEnumerable<User>> GetUsers();

        Task<User> CreateUser(User user);

        Task<Boolean> DeleteUser(int userId);

        Task<IEnumerable<User>> GetUserFriends(int userId);

        Task<Boolean> RemoveFriendship(int sourceUserId, int targetUserId);

        Task<IEnumerable<User>> GetBlockedUsers(int userId);

        Task<Boolean> BlockUser(int sourceUserId, int targetUserId);

        Task<Boolean> UnblockUser(int sourceUserId, int targetUserId);

        Task<IEnumerable<User>> GetSentFriendRequests(int userId);

        Task<Boolean> SendFriendRequest(int sourceUserId, int targetUserId);

        Task<Boolean> UnsendFriendRequest(int sourceUserId, int targetUserId);

        Task<IEnumerable<User>> GetReceivedFriendRequests(int userId);

        Task<Boolean> AcceptFriendRequest(int sourceUserId, int targetUserId);

        Task<Boolean> DeclineFriendRequest(int sourceUserId, int targetUserId);
    }
}
