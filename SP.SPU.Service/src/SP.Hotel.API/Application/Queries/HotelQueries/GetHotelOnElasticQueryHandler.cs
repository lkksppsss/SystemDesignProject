using AutoMapper;
using MediatR;
using SP.SPU.API.ViewModels;
using SP.SPU.Infrastructure.ElasticSearch;
using SP.SPU.Infrastructure.ElasticSearch.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SP.SPU.API.Application.Queries.HotelQueries;

public class GetHotelOnElasticQueryHandler : IRequestHandler<GetHotelOnElasticQuery, List<HotelVo>>
{
    private readonly IElasticsearchService _elasticsearch;
    private readonly IMapper _mapper;

    public GetHotelOnElasticQueryHandler(IElasticsearchService elasticsearch, IMapper mapper)
    {
        _elasticsearch = elasticsearch;
        _mapper = mapper;
    }
    public async Task<List<HotelVo>> Handle(GetHotelOnElasticQuery request, CancellationToken cancellationToken)
    {
        var req = new ElasticsearchSearchRequest();
        req.Keywords = request.Keyword.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var search = await _elasticsearch.SearchAsync(req);
        var result = _mapper.Map<List<HotelVo>>(search);
        return result;
    }
}
