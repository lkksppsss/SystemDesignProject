using AutoMapper;
using SP.SPU.API.Application.Commands;
using SP.SPU.API.Application.Models;
using SP.SPU.Domian.AggregatesModel.HotelAggregate;

namespace SP.SPU.API.Application.Profiles;

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
