using System.Text.Json.Serialization;

namespace SPCorePackage.Kafka;

public abstract record IntegrationEvent
{        
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreateDate = DateTime.Now;
    }

    [JsonConstructor]
    public IntegrationEvent(Guid id, DateTime createDate)
    {
        Id = id;
        CreateDate = createDate;
    }

    [JsonInclude]
    public Guid Id { get; set; }

    [JsonInclude]
    public DateTime CreateDate { get; set; }

    [JsonInclude]
    public string ActivityId { get; set; }

    [JsonInclude]
    public long Offset { get; set; }

}
