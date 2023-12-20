using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCorePackage.Kafka.Interface;

public interface IEventBus
{
    Task PublishAsync<T>(string exchangeName, T @event) where T : IntegrationEvent;
    void Subscribe<T, TH>(string topicName, string groupId = null)
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
}
