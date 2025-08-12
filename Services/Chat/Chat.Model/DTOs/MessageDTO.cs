namespace Chat.Model.DTOs;
public class ChatMessageDTO
{
  public bool IsSender{ get; set; }= false;
  public string Message{ get; set; } = string.Empty;
}