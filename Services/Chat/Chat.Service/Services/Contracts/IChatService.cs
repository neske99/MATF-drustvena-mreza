
using Services.Chat.Chat.Model.Entities;

namespace Chat.Service.Services.Contracts

{
    public interface IChatService
    {
        Task<List<ChatUser>> GetUsersForGroupAsync(int userId, int chatGroupId);
        Task<List<ChatMessage>> GetMessagesForGroupAsync(int userId, int chatGroupId);
        Task<List<ChatGroup>> GetChatGroupForGroupAsync(int userId);
        Task<List<User>> GetAllUsers();


    }
}