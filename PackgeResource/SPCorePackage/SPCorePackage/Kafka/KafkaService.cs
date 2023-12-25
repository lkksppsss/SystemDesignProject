using Autofac;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SPCorePackage.Kafka.Interface;
using System.Text;
using System.Text.Json;

namespace SPCorePackage.Kafka;

public class KafkaService : IEventBus
{
    private ProducerBuilder<string, object> ProducerBuilder;
    private ConsumerBuilder<string, object> ConsumerBuilder;
    private List<string> _topicNames;
    private IAdminClient _kafkaConnection;
    private ConsumerConfig _consumerConfig;
    private readonly CancellationTokenSource _cancelTokenSource;
    private readonly ILifetimeScope _autofac;

    public KafkaService(string service, ILifetimeScope autofac, params string[] topics)
    {
        ProducerConfig producerConfig = new ProducerConfig();
        producerConfig.BootstrapServers = service;
        _autofac = autofac;
        ProducerBuilder = new ProducerBuilder<string, object>(producerConfig);
        ProducerBuilder.SetValueSerializer(new KafkaConverter());//设置序列化方式
        _cancelTokenSource = new CancellationTokenSource();
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = service,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            GroupId = "group"
        };

        var adminConfig = new AdminClientConfig
        {
            BootstrapServers = service
        };
        _kafkaConnection = new AdminClientBuilder(adminConfig).Build();
        ConsumerBuilder = new ConsumerBuilder<string, object>(_consumerConfig);
        _topicNames = CreateTopics(topics);
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

                Message<string, object> msg = new Message<string, object>();
                msg.Key = guid;
                msg.Value = @event;

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


    public void Subscribe<T, TH>(string topicName)
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        bool errorSent = false;
        while (this._topicNames.Contains(topicName) == false)
        {
            if (errorSent == false)
            {
                errorSent = true;
            }
            Task.Delay(10000).Wait();
            this._topicNames = GetTopics();
        }
        StartBasicConsume<T, TH>(topicName, "group").Wait();
    }
    private async Task StartBasicConsume<T, TH>(string topicName, string groupId)
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        TH handler = _autofac.Resolve<TH>();
        var consumerService = new KafkaConsumerService<T, TH>(_autofac.Resolve<ILogger<KafkaConsumerService<T, TH>>>(), GetConsumer(groupId), topicName, _autofac);

        await consumerService.StartAsync(_cancelTokenSource.Token);
    }

    public List<string> GetTopics()
    {

        var metadata = _kafkaConnection.GetMetadata(TimeSpan.FromSeconds(10));
        var topicsMetadata = metadata.Topics;
        return metadata.Topics.Select(a => a.Topic).ToList();
    }

    private List<string> CreateTopics(params string[] createTopicOptions)
    {
        //建立topic
        var allTopicNames = new List<string>();

        var metadata = _kafkaConnection.GetMetadata(TimeSpan.FromSeconds(10));
        var topicsMetadata = metadata.Topics;
        var topicNames = metadata.Topics.Select(a => a.Topic).ToList();
        allTopicNames.AddRange(topicNames);

        if (createTopicOptions != null)
        {
            var newTopicSpecs = new List<TopicSpecification>();

            foreach (var option in createTopicOptions)
            {
                if (topicNames.Contains(option))
                {
                    continue;
                }
                var newTopicSpec = new TopicSpecification
                {
                    Name = option
                };
                newTopicSpecs.Add(newTopicSpec);
            }
            try
            {
                if (newTopicSpecs.Count > 0)
                {
                    _kafkaConnection.CreateTopicsAsync(newTopicSpecs).Wait();
                    allTopicNames.AddRange(newTopicSpecs.Select(x => x.Name));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured creating topic ex={ex}");
            }
        }
        return allTopicNames;
    }

    public IConsumer<string, string> GetConsumer(string groupId)
    {
        var config = new ConsumerConfig(_consumerConfig);
        config.GroupId = groupId;
        return new ConsumerBuilder<string, string>(config).Build();
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

}
