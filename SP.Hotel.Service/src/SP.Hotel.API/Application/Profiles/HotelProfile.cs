using AutoMapper;
using SP.Hotel.API.Application.Commands;
using SP.Hotel.API.Application.Models;
using SP.Hotel.Domian.AggregatesModel.HotelAggregate;

namespace SP.Hotel.API.Application.Profiles;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<CreateHotelCommand, CreateHotelModel>();
        CreateMap< CreateHotelPicture, ICreateHotelPicture>().As<CreateHotelPictureModel>();
        CreateMap<CreateHotelPicture, CreateHotelPictureModel>()
            .ForMember(x => x.Type, y => y.MapFrom(a => HotelPicType.From(a.Type)));
    }
}
