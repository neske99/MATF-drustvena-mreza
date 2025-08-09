using Services.Chat.Chat.Model.Entities.Common;

namespace Services.Chat.Chat.Model.Entities;
public class ChatMessage:EntityBase
{
  public int UserId{ get; set; }
  public int ChatGroupId { get; set; }
  public string Text { get; set; }
}