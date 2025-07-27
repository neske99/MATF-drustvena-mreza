namespace Post.Application.DTOs
{
    public class CreatePostDTO
    {
        public string Text { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Picture { get; set; } = string.Empty;
        public List<CreateCommentDTO> Comments { get; set; } = new List<CreateCommentDTO>();
    }
}