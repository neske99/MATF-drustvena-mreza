
using Chat.Model.DTOs;
using Services.Chat.Chat.Model.Entities;

namespace Chat.Service.Services.Contracts

{
    public interface IChatService
    {
        Task<List<ChatUser>> GetUsersForGroupAsync(int userId, int chatGroupId);
        Task<List<ChatMessageDTO>> GetMessagesForGroupAsync(int userId, int chatGroupId);
        Task<List<ChatGroupDTO>> GetChatGroupForGroupAsync(int userId);
        Task<List<User>> GetAllUsers();

        Task<ChatMessage> CreateMessageForChatGroupAsync(int userId, int chatGroupId, string message);

        Task<ChatGroup> CreateChatGroupAsync(int userAId, int userBId);
    }
}