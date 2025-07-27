namespace EventBusMessages.Events;

public class UserCreatedEvent : IntegrationBaseEvent
{
    //User
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }


}
