namespace Post.Application.DTOs
{
    public class CreatePostDTO
    {
        public string Text { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public List<CreateCommentDTO> Comments { get; set; } = new List<CreateCommentDTO>();
    }
}