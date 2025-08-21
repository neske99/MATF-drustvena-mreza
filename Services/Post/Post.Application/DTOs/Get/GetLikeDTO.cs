namespace Post.Application.DTOs
{
    public class GetLikeDTO
    {
        public int Id { get; set; }
        public GetUserDTO? User { get; set; }
    }
}