namespace Services.Chat.Chat.Model.Entities.Common;
public class EntityBase
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    // public int CreatedById { get; set; }
    public int? LastModifiedBy { get; set; }
}