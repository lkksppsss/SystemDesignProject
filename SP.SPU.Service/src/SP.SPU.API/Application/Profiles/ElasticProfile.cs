using AutoMapper;
using SP.SPU.API.ViewModels;
using SP.SPU.Domain.AggregatesModel.HotelAggregate;
using SP.SPU.Infrastructure.ElasticSearch.Models;

namespace SP.SPU.API.Application.Profiles;

public class ElasticProfile : Profile
{
    public ElasticProfile()
    {
        CreateMap<HotelEntity, ElasticsearchHotelModel>(); 
        CreateMap<HotelPictureEntity, ElasticsearchHotelPiceture>()
            .ForMember(x => x.Type , y => y.MapFrom(a => a.Type.Id)); 

        CreateMap<ElasticsearchHotelModel,HotelVo>(); 
        CreateMap<ElasticsearchHotelPiceture, HotelPicetureVo>(); 
    }
}
