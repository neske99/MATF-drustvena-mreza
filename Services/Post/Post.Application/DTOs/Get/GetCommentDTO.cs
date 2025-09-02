namespace Post.Application.DTOs
{
    public class GetCommentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public GetUserDTO? User { get; set; }
    }
}