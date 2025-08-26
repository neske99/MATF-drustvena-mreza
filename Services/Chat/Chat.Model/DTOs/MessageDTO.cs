namespace Chat.Model.DTOs;
public class ChatMessageDTO
{
  public int Id { get; set; }
  public bool IsSender { get; set; } = false;
  public string Message{ get; set; } = string.Empty;
  public DateTime Timestamp { get; set; }
}