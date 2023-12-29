using AutoMapper;
using MediatR;
using SP.SPU.Domain.AggregatesModel.HotelAggregate;
using SP.SPU.Infrastructure.ElasticSearch;
using SP.SPU.Infrastructure.ElasticSearch.Models;

namespace SP.SPU.API.Application.Commands;

public class AddDataToElasticCommandHandler : IRequestHandler<AddDataToElasticCommand, bool>
{
    private readonly IElasticsearchService _elasticsearch;
    private readonly IMapper _mapper;
    private readonly IHotelRepository _hotelRepo;

    public AddDataToElasticCommandHandler(IElasticsearchService elasticsearch, IMapper mapper, IHotelRepository hotelRepo)
    {
        _elasticsearch = elasticsearch;
        _mapper = mapper;
        _hotelRepo = hotelRepo;
    }

    public async Task<bool> Handle(AddDataToElasticCommand request, CancellationToken cancellationToken)
    {
        var hotelEntity = await _hotelRepo.GetAsync(request.Id);

        var elasticModel = _mapper.Map<ElasticsearchHotelModel>(hotelEntity);
        await _elasticsearch.Insert(elasticModel);

        return true;
    }
}
