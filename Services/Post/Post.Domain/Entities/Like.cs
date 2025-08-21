using Post.Domain.Common;

namespace Post.Domain.Entities;

public class Like : Aggregate
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}