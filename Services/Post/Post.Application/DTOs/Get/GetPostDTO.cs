namespace Post.Application.DTOs
{
    public class GetPostDTO
    {

        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public GetUserDTO? User { get; set; }
        public string Picture { get; set; } = string.Empty;
        public List<GetCommentDTO> Comments { get; set; } = new List<GetCommentDTO>();
        public List<GetLikeDTO> Likes { get; set; } = new List<GetLikeDTO>();
    }
}