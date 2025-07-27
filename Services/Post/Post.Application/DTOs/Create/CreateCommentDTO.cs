namespace Post.Application.DTOs
{
    public class CreateCommentDTO
    {
        public string Text { get; set; }=string.Empty;
        public int UserId { get; set; }
    }
}