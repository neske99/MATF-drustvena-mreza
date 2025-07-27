using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Domain.Common;

namespace Post.Domain.Entities;

public class Comment : Aggregate
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public string Text { get; set; } = string.Empty;


}
