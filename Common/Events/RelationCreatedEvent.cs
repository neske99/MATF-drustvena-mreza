namespace EventBusMessages.Events;

public class RelationCreatedEvent : IntegrationBaseEvent
{
  //User
  public int UserAId { get; set; }
  public int UserBId { get; set; }

}