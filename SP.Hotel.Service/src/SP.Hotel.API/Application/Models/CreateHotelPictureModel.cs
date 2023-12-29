using SP.Hotel.Domain.AggregatesModel.HotelAggregate;

namespace SP.Hotel.API.Application.Models;

public class CreateHotelPictureModel : ICreateHotelPicture
{
    public HotelPicType Type { get; set; }
    public string Url { get; set; }
}
