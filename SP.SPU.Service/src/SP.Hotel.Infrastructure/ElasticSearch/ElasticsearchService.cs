using MediatR;
using Nest;
using Newtonsoft.Json;
using SP.SPU.Infrastructure.ElasticSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.SPU.Infrastructure.ElasticSearch;

public class ElasticsearchService : IElasticsearchService
{
    private readonly ElasticClient _elasticClient;
    public ElasticsearchService()
    {
        var node = new Uri("http://127.0.0.1:9200");
        var settings = new ConnectionSettings(node);
        _elasticClient = new ElasticClient(settings);
    }
    public void Insert(ElasticsearchHotelModel houseDTO)
    {
        var response = _elasticClient.Index(houseDTO, idx => idx.Index("hotel"));
    }
    public List<ElasticsearchHotelModel> Search(ElasticsearchSearchRequest request)
    {
        QueryContainer queryContainer = null;

        foreach (var item in request.Keywords)
        {
            var toLowerItem = item.ToLower();

            var hotelNameQuery = new TermQuery();
            hotelNameQuery.Field = "hotel_name";
            hotelNameQuery.Value = toLowerItem;
            hotelNameQuery.Boost = 40; // 欄位權重, 需調教測試
            queryContainer |= hotelNameQuery;

            var cityQuery = new TermQuery();
            cityQuery.Field = "city";
            cityQuery.Value = toLowerItem;
            cityQuery.Boost = 20; // 欄位權重, 需調教測試
            queryContainer |= cityQuery;

            var areaQuery = new TermQuery();
            areaQuery.Field = "area";
            areaQuery.Value = item;
            areaQuery.Boost = 100; // 欄位權重, 需調教測試
            queryContainer |= areaQuery;

            var addressQuery = new TermQuery();
            addressQuery.Field = "address";
            addressQuery.Value = toLowerItem;
            addressQuery.Boost = 5; // 欄位權重, 需調教測試
            queryContainer |= addressQuery;
        }

        var response = _elasticClient.Search<ElasticsearchHotelModel>(s => s
            .Index("hotel")
            .From(0)
            .Size(10)
            .Query(q => q
                .Bool(b => b.Should(queryContainer)))
            );

        var list = response.Documents;
        return list.ToList();
    }
}
