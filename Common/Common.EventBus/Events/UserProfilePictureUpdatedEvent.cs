namespace EventBusMessages.Events;

public class UserProfilePictureUpdatedEvent : IntegrationBaseEvent
{
    public int UserId { get; set; }
    public string? ProfilePictureUrl { get; set; }
}