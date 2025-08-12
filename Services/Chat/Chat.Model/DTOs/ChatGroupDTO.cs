namespace Chat.Model.DTOs;
public class ChatGroupDTO
{
  public int ChatId{ get; set; }
  public int UserId { get; set; }
  public string Username { get; set; } = string.Empty;
}