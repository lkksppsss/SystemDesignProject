using SP.Hotel.Domian.SeedWork;

namespace SP.Hotel.Domian.AggregatesModel.HotelAggregate;

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
