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
        public string Picture { get; set; } = string.Empty;
        public List<Comment> Comments { get; set; } = new List<Comment>() { };


    }
}