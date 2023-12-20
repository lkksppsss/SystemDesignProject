using SP.Hotel.API.Application.IntegrationEvents.PublishEvents;
using SPCorePackage.Kafka.Interface;

namespace SP.Hotel.API.Application.IntegrationEvents.EventHandling;

public class TestPublishEventHandler : IIntegrationEventHandler<TestPublishEvent>
{
    public async Task Handle(TestPublishEvent @event)
    {
        var name = @event.Name;
    }
}
