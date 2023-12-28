namespace SPCorePackage.Kafka.Interface;

public interface IEventBus
{
    Task PublishAsync<T>(string exchangeName, T @event) where T : IntegrationEvent;
    void Subscribe<T, TH>(string topicName)
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
}
