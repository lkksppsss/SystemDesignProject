using Autofac;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SPCorePackage.Kafka.Interface;
using System.Diagnostics;
using System.Text.Json;
using static System.Formats.Asn1.AsnWriter;

namespace SPCorePackage.Kafka;

public class KafkaConsumerService<T, TH> : BackgroundService
    where T : IntegrationEvent
    where TH : IIntegrationEventHandler<T>
{
    private ILogger<KafkaConsumerService<T, TH>> _logger;
    ILifetimeScope _autofac;
    private IConsumer<string, string> _consumer;
    private string _topicName;
    Type _eventHandlerType;
    Type _eventType;
    public KafkaConsumerService(ILogger<KafkaConsumerService<T, TH>> logger, IConsumer<string, string> consumer,string topicName, ILifetimeScope scope)
    {
        _logger = logger;
        _consumer = consumer;
        _topicName = topicName;
        _eventHandlerType = typeof(TH);
        _eventType = typeof(T);
        _autofac = scope;
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

        using (var scope = _autofac.BeginLifetimeScope("SP"))
        {
            var handler = scope.ResolveOptional(_eventHandlerType);
            if (handler == null)
            {
                _logger.LogError($"Autofac resolve {_eventHandlerType} error.");
                return;
            }

            try
            {
                T integrationEvent = JsonSerializer.Deserialize(
                    message,
                    _eventType,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) as T;
                integrationEvent.Offset = offset;

                await (Task)_eventHandlerType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
            }
            catch (Exception ex)
            {
                string errorMessage = $"ERROR ProcessEvent {_eventHandlerType.Name}, {ex.Message}, {ex}";
                throw;
            }
        }
            
    }
}