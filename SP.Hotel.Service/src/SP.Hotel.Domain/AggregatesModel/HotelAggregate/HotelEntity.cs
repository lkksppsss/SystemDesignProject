using SP.Hotel.Domain.SeedWork;

namespace SP.Hotel.Domain.AggregatesModel.HotelAggregate;

public class HotelEntity : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string City { get; private set; }
    public string Area { get; private set; }
    public string Address { get; private set; }
    public string Description { get; private set; }
    public DateTime CreateTime { get; private set; }
    public List<HotelPictureEntity> Pictures { get; private set; }
    public HotelEntity()
    {
        Pictures = new List<HotelPictureEntity>();
    }
    public HotelEntity(ICreateHotel data)
    {
        Name = data.Name;
        City = data.City;
        Area = data.Area;
        Address = data.Address;
        Description = data.Description;
        CreateTime = DateTime.Now;

        Pictures = new List<HotelPictureEntity>();
        data.Pictures.ForEach(x => { Pictures.Add(new HotelPictureEntity(x)); });
    }
}
