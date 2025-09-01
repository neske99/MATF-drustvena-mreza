using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Post.Domain.Common;

namespace Post.Domain.Entities
{
    public class Post : Aggregate
    {
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        public string Text { get; set; } = string.Empty;

        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>() { };
        public List<Like> Likes { get; set; } = new List<Like>() { };


    }
}