namespace EventBus.Messages;

public record IntegrationBaseEvent():IIntegrationEvent
{
    public  DateTime CreationDate { get; } = DateTime.Now;
    public Guid Id { get; set; }
}