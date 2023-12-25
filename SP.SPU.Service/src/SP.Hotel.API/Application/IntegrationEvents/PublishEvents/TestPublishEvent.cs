using SPCorePackage.Kafka;

namespace SP.SPU.API.Application.IntegrationEvents.PublishEvents;

public record TestPublishEvent : IntegrationEvent
{
    public const string EventName = "SP.TestPublishEvent";
    public string Name {  get; set; }
    public DateTime Time { get; set; }
    public bool IsBool { get; set; }
    public int Number {  get; set; }
}
