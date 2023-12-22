using Autofac;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SPCorePackage.Kafka.Interface;

namespace SPCorePackage.Kafka;

public class KafkaConsumerService<T, TH> : BackgroundService
    where T : IntegrationEvent
    where TH : IIntegrationEventHandler<T>
{
    private IConsumer<string, string> _consumer;
    private string _topicName;
    public KafkaConsumerService(IConsumer<string, string> consumer,string topicName)
    {
        _consumer = consumer;
        _topicName = topicName;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        _consumer.Subscribe(_topicName);

        while (stoppingToken.IsCancellationRequested == false)
        {
            long offset = -1;
            ConsumeResult<string, string> consumeResult = _consumer.Consume(stoppingToken);
            offset = consumeResult.Offset.Value;
            try
            {
                await ProcessEvent(_topicName, consumeResult.Value, offset, consumeResult.Message.Key);
                _consumer.Commit();
            }
            catch(Confluent.Kafka.KafkaException kex) when (kex.Error?.Reason == "Local: No offset stored")
            {
                // Local: No offset stored = consumer did not see a point in committing any offsets, the offsets are already committed
            }
            catch (Exception ex)
            {
                await StopAsync(stoppingToken);
            }
        }
    }

    private async Task ProcessEvent(string eventName, string message, long offset, string messageKey)
    {
    }
}