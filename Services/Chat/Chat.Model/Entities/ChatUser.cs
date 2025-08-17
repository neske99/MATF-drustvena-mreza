using Services.Chat.Chat.Model.Entities.Common;

namespace Services.Chat.Chat.Model.Entities;

public class ChatUser : EntityBase
{
  public int ChatGroupId { get; set; }
  public int UserId { get; set; }
  public bool isActive{ get; set; }
  public User? User { get; set; }
  public bool hasNewMessages { get; set; } = false;
}