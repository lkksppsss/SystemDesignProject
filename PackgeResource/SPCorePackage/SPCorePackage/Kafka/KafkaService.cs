using Confluent.Kafka;
using Newtonsoft.Json;
using SPCorePackage.Kafka.Interface;
using System.Text;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace SPCorePackage.Kafka;

public class KafkaService : IEventBus
{
    private ProducerBuilder<string, object> ProducerBuilder;
    private ConsumerBuilder<string, object> ConsumerBuilder;
    public KafkaService(string service)
    {
        ProducerConfig producerConfig = new ProducerConfig();
        producerConfig.BootstrapServers = service;
        ProducerBuilder = new ProducerBuilder<string, object>(producerConfig);
        ProducerBuilder.SetValueSerializer(new KafkaConverter());//设置序列化方式

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = service,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
        ConsumerBuilder = new ConsumerBuilder<string, object>(consumerConfig);
        ConsumerBuilder.SetValueDeserializer(new KafkaConverter());//设置反序列化方式
    }

    public async Task PublishAsync<T>(string exchangeName, T @event) where T : IntegrationEvent
    {
        await PublishAsync(exchangeName, new List<T> { @event });
    }
    public async Task PublishAsync<T>(string exchangeName, List<T> events) where T : IntegrationEvent
    {
        for (int i = 0; i < events.Count; i++)
        {
            var producer = ProducerBuilder.Build();
            try
            {
                var guid = Guid.NewGuid().ToString("N");
                var @event = events[i];

                var body = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented // 設定 Formatting 屬性為 Indented
                });

                Message<string, object> msg = new Message<string, object>();
                msg.Key = guid;
                msg.Value = body;

                var res = await producer.ProduceAsync(exchangeName, msg);
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }
    }


    public void Subscribe<T, TH>(string topicName, string groupId = null)
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {

        var consumer = ConsumerBuilder.Build();
        consumer.Subscribe(topicName);
        bool errorSent = false;
        while (true)
        {
            if (errorSent == false)
            {
                errorSent = true;
            }
            Task.Delay(10000).Wait();
            var result = consumer.Consume();
            consumer.Commit(result);
        }
    }

}
public class KafkaConverter : ISerializer<object>, IDeserializer<object>
{
    /// <summary>
    /// 序列化数据成字节
    /// </summary>
    /// <param name="data"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public byte[] Serialize(object data, SerializationContext context)
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented // 設定 Formatting 屬性為 Indented
        };
        var json = JsonConvert.SerializeObject(data, settings);
        return Encoding.UTF8.GetBytes(json);
    }

    /// <summary>
    /// 反序列化字节数据成实体数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public object Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull) return null;

        var json = Encoding.UTF8.GetString(data.ToArray());
        try
        {
            return JsonConvert.DeserializeObject(json);
        }
        catch
        {
            return json;
        }
    }
}
