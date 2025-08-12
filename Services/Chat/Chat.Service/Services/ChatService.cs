using Chat.Model.DTOs;
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

    public async Task<List<ChatGroupDTO>> GetChatGroupForGroupAsync(int userId)
    {
          return  await _chatRepository.GetChatGroupForUserAsync(userId);
    }

    public async Task<List<ChatMessageDTO>> GetMessagesForGroupAsync(int userId, int chatGroupId)
    {
          return await _chatRepository.GetMessagesForGroupAsync(userId, chatGroupId);
    }

    public async Task<List<ChatUser>> GetUsersForGroupAsync(int userId, int chatGroupId)
    {
        return await _chatRepository.GetUsersForGroupAsync(userId, chatGroupId);
    }

    public async Task<bool> CreateMessageForChatGroupAsync(int userId, int chatGroupId,string message)
    {
        return await _chatRepository.CreateMessageForChatGroupAsync(userId, chatGroupId,message);
    }

    public async Task<ChatGroup> CreateChatGroupAsync(int userAId, int userBId)
    {
      return await _chatRepository.CreateChatGroupAsync(userAId, userBId);

    }
  }
}