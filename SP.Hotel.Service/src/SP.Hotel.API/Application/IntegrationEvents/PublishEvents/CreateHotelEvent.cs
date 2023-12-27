using SP.Hotel.Domian.AggregatesModel.HotelAggregate;
using SPCorePackage.Kafka;

namespace SP.Hotel.API.Application.IntegrationEvents.PublishEvents;

public record CreateHotelEvent : IntegrationEvent
{
    public const string EventName = "SP.CreateHotelEventt";
    public int HotelId { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Area { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public List<HotelPiceture> Pictures { get; set; }
    public CreateHotelEvent(HotelEntity entity)
    {
        HotelId = entity.Id;
        Name = entity.Name;
        City = entity.City;
        Area = entity.Area;
        Address = entity.Address;
        Description = entity.Description;
        Pictures = new List<HotelPiceture>();
        entity.Pictures.ForEach(x =>
        {
            Pictures.Add(new HotelPiceture(x));
        });
    }
}

public class HotelPiceture
{
    /// <summary>
    /// 相片類型
    /// 0: 預設
    /// 1: 房間
    /// 2: 設施
    /// 3: 建築物
    /// </summary>
    public int Type { get; set; }
    public string Url { get; set; }

    public HotelPiceture(HotelPictureEntity entity)
    {
        Type = entity.Type.Id;
        Url = entity.Url;
    }
}
