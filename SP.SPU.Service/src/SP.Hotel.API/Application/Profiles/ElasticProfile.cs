using AutoMapper;
using SP.SPU.Domian.AggregatesModel.HotelAggregate;
using SP.SPU.Infrastructure.ElasticSearch.Models;

namespace SP.SPU.API.Application.Profiles;

public class ElasticProfile : Profile
{
    public ElasticProfile()
    {
        CreateMap<HotelEntity, ElasticsearchHotelModel>(); 
    }
}
