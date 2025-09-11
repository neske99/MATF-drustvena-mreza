using Microsoft.AspNetCore.Http;

namespace Post.API.DTOs
{
    public class CreatePostFormDTO
    {
        public int UserId { get; set; }
        public string Text { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
    }
}