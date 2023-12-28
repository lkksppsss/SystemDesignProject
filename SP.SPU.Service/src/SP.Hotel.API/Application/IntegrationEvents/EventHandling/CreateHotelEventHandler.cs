using AutoMapper;
using SP.SPU.API.Application.IntegrationEvents.PublishEvents;
using SP.SPU.API.Application.Models;
using SP.SPU.Domian.AggregatesModel.HotelAggregate;
using SP.SPU.Infrastructure.ElasticSearch;
using SP.SPU.Infrastructure.ElasticSearch.Models;
using SPCorePackage.Kafka.Interface;

namespace SP.SPU.API.Application.IntegrationEvents.EventHandling;

public class CreateHotelEventHandler : IIntegrationEventHandler<CreateHotelEvent>
{
    private readonly IHotelRepository _hotelRepo;
    private readonly IMapper _mapper;
    private readonly IElasticsearchService _elasticService;
    public CreateHotelEventHandler(IHotelRepository hotelRepo, IMapper mapper, IElasticsearchService elasticService)
    {
        _hotelRepo = hotelRepo;
        _mapper = mapper;
        _elasticService = elasticService;
    }

    public async Task Handle(CreateHotelEvent @event)
    {
        try
        {
            var hotelModel = _mapper.Map<CreateHotelModel>(@event);
            var hotelEntity = new HotelEntity(hotelModel);
            _hotelRepo.Add(hotelEntity);

            if (await _hotelRepo.UnitOfWork.SaveEntitiesAsync() > 0)
            {
                var elasticModel = _mapper.Map<ElasticsearchHotelModel>(hotelEntity);
                _elasticService.Insert(elasticModel);
            }
        }
        catch(Exception ex)
        {

        }
    }
}
