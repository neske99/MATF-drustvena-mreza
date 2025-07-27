namespace Post.Application.DTOs
{
    public class GetCommentDTO
    {
        public string Text { get; set; }=string.Empty;
        public GetUserDTO? User { get; set; }
    }
}