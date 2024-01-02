using AutoMapper;
using SP.SPU.API.Application.Dto;
using SP.SPU.API.Application.IntegrationEvents.PublishEvents;
using SP.SPU.API.Application.Models;
using SP.SPU.API.ViewModels;
using SP.SPU.Domian.AggregatesModel.HotelAggregate;

namespace SP.SPU.API.Application.Profiles;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<CreateHotelEvent, CreateHotelModel>();
        CreateMap<HotelPiceture, ICreateHotelPicture>().As<CreateHotelPictureModel>();
        CreateMap<HotelPiceture, CreateHotelPictureModel>()
            .ForMember(x => x.Type, y => y.MapFrom(a => HotelPicType.From(a.Type)));
        CreateMap<HotelDto, HotelVo>();
        CreateMap<HotelPicetureDto, HotelPicetureVo>();
    }
}
