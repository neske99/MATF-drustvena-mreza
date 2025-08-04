using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    public ChatController()
    {
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<ActionResult> Test()
    {
      return Ok("Chat API is running successfully!");
    }
}