using Services.Chat.Chat.Model.Entities.Common;

namespace Services.Chat.Chat.Model.Entities;
public class ChatGroup:EntityBase
{
  public List<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();
}