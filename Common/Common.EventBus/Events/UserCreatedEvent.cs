namespace EventBusMessages.Events;

public class UserCreatedEvent : IntegrationBaseEvent
{
    //User
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}