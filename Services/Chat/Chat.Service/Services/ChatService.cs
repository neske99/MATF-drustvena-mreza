using Chat.Repository.Repositories.Contracts;
using Chat.Service.Services.Contracts;
using Services.Chat.Chat.Model.Entities;

namespace Chat.Service.Services

{
    public class ChatService : IChatService
    {
      private readonly IChatRepository _chatRepository;

      public ChatService(IChatRepository chatRepository)
      {
          _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
      }

    public async Task<List<User>> GetAllUsers()
    {
      return await _chatRepository.GetAllUsers();
    }

    public async Task<List<ChatGroup>> GetChatGroupForGroupAsync(int userId)
    {
          return  await _chatRepository.GetChatGroupForUserAsync(userId);
    }

    public async Task<List<ChatMessage>> GetMessagesForGroupAsync(int userId, int chatGroupId)
    {
          return await _chatRepository.GetMessagesForGroupAsync(userId, chatGroupId);
    }

    public async Task<List<ChatUser>> GetUsersForGroupAsync(int userId, int chatGroupId)
    {
        return await _chatRepository.GetUsersForGroupAsync(userId, chatGroupId);
    }
  }
}