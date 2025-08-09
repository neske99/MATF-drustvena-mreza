using Services.Chat.Chat.Model.Entities;

namespace Chat.Repository.Repositories.Contracts

{
    public interface IChatRepository
    {
        Task<List<ChatMessage>> GetMessagesForGroupAsync(int userId, int chatGroupId);
        Task<List<ChatUser>> GetUsersForGroupAsync(int userId, int chatGroupId);

        Task<List<ChatGroup>> GetChatGroupForUserAsync(int userId);
        Task<List<User>> GetAllUsers();

        Task<bool> ReplicateUser(User user);

    }
}