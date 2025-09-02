using System;

namespace Post.Application.DTOs
{
    public class GetPostDTO
    {

        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public GetUserDTO? User { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }

        public List<GetCommentDTO> Comments { get; set; } = new List<GetCommentDTO>();
        public List<GetLikeDTO> Likes { get; set; } = new List<GetLikeDTO>();
    }
}