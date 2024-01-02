using SP.SPU.Domain.SeedWork;

namespace SP.SPU.Domain.AggregatesModel.HotelAggregate;

public class HotelPictureEntity : Entity
{
    public HotelPicType Type { get; private set; }
    public string Url { get; private set; }
    public DateTime CreateTime { get; private set; }
    public HotelPictureEntity() { }
    public HotelPictureEntity(ICreateHotelPicture data)
    {
        Type = data.Type;
        Url = data.Url;
        CreateTime = DateTime.Now;
    }
}
