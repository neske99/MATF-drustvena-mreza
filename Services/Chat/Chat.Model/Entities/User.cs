using Services.Chat.Chat.Model.Entities.Common;

namespace Services.Chat.Chat.Model.Entities;

public class User : EntityBase
{
  public string Username { get; set; }= string.Empty;
}