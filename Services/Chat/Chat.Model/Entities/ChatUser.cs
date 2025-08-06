namespace Services.Chat.Chat.Model.Entities;

public class ChatUser : EntityBase
{
  public int ChatGroupId { get; set; }
  public int UserId { get; set; }
  public bool isActive{ get; set; }
}