using SP.SPU.API.Application.IntegrationEvents.PublishEvents;
using SPCorePackage.Kafka.Interface;

namespace SP.SPU.API.Application.IntegrationEvents.EventHandling;

public class TestPublishEventHandler : IIntegrationEventHandler<TestPublishEvent>
{
    public async Task Handle(TestPublishEvent @event)
    {
        var name = @event.Name;
    }
}
