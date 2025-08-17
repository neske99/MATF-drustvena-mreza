using Chat.Model.DTOs;
using Services.Chat.Chat.Model.Entities;

namespace Chat.Repository.Repositories.Contracts

{
    public interface IChatRepository
    {
        Task<List<ChatMessageDTO>> GetMessagesForGroupAsync(int userId, int chatGroupId);
        Task<List<ChatUser>> GetUsersForGroupAsync(int userId, int chatGroupId);

        Task<List<ChatGroupDTO>> GetChatGroupForUserAsync(int userId);
        Task<List<User>> GetAllUsers();

        Task<bool> ReplicateUser(User user);

        Task<bool> CreateMessageForChatGroupAsync(int userId, int chatGroupId, string message);

        Task<ChatGroup> CreateChatGroupAsync(int userAId, int userBId);

        Task SetChatUsersHasNewMessagesAsync(int chatGroupId, int userId);
    }
}