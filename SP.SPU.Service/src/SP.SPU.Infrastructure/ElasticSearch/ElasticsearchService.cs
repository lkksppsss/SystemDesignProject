﻿using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;
using SP.SPU.Infrastructure.ElasticSearch.Models;
using SP.SPU.Infrastructure.Models;

namespace SP.SPU.Infrastructure.ElasticSearch;

public class ElasticsearchService : IElasticsearchService
{
    private readonly ElasticClient _elasticClient;
    private readonly ElasticSetting _elasticSetting;
    public ElasticsearchService(IOptions<ElasticSetting> elasticSetting)
    {
        _elasticSetting = elasticSetting.Value;
        var node = new Uri(_elasticSetting.ConnectString);
        var settings = new ConnectionSettings(node);
        _elasticClient = new ElasticClient(settings);
    }
    public async Task CreateIndex()
    {
        var synonymFilterName = "my_synonym";
        var nGramFilterName = "my_ngram";
        var lowercase = "lowercase";
        var response = await  _elasticClient.Indices.CreateAsync(_elasticSetting.Index, c => c
            .Settings(s => s
                .Setting("index.max_ngram_diff", 8)
                .Analysis(a => a
                    // 自訂同義字方法
                    .Analyzers(an => an.Custom("ik_search_analyzer", ca => ca.Tokenizer("ik_max_word"))
                        .Custom("ngram_analyzer", ca => ca.Tokenizer(nGramFilterName).Filters(lowercase)))
                    .Tokenizers(tz => tz.NGram(nGramFilterName, td => td.MaxGram(8)
                        .MinGram(1)
                        .TokenChars(TokenChar.Letter, TokenChar.Digit, TokenChar.Punctuation, TokenChar.Symbol)))
                    ))
            .Map<ElasticsearchHotelModel>(m => m.AutoMap()));
    }
    public async Task Insert(ElasticsearchHotelModel houseDTO)
    {
        var response = await _elasticClient.IndexAsync(houseDTO, b => b.Index(_elasticSetting.Index));
    }
    public async Task<List<ElasticsearchHotelModel>> SearchAsync(ElasticsearchSearchRequest request)
    {
        QueryContainer queryContainer = null;

        foreach (var item in request.Keywords)
        {
            var toLowerItem = item.ToLower();

            var hotelNameQuery = new TermQuery();
            hotelNameQuery.Field = "hotel_name";
            hotelNameQuery.Value = toLowerItem;
            hotelNameQuery.Boost = 100; // 欄位權重
            queryContainer |= hotelNameQuery;

            var cityQuery = new TermQuery();
            cityQuery.Field = "city";
            cityQuery.Value = toLowerItem;
            cityQuery.Boost = 20; // 欄位權重
            queryContainer |= cityQuery;

            var areaQuery = new TermQuery();
            areaQuery.Field = "area";
            areaQuery.Value = item;
            areaQuery.Boost = 30; // 欄位權重
            queryContainer |= areaQuery;

            var addressQuery = new TermQuery();
            addressQuery.Field = "address";
            addressQuery.Value = toLowerItem;
            addressQuery.Boost = 10; // 欄位權重
            queryContainer |= addressQuery;

            var descriptionQuery = new TermQuery();
            descriptionQuery.Field = "description";
            descriptionQuery.Value = toLowerItem;
            descriptionQuery.Boost = 5; // 欄位權重
            queryContainer |= descriptionQuery;
        }
        var req = new SearchRequest<ElasticsearchHotelModel>(_elasticSetting.Index)
        {
            From = 0,
            Size = 10,
            Query = new BoolQuery
            {
                Should = new List<QueryContainer> { queryContainer }
            }
        };

        string jsonString = _elasticClient.RequestResponseSerializer.SerializeToString(req);

        var searchDescriptor = new SearchDescriptor<ElasticsearchHotelModel>()
            .Index("hotel")
            .From(0)
            .Size(10)
            .Query(q => q
                .Bool(b => b.Should(queryContainer)
            ));

        var response = await _elasticClient.SearchAsync<ElasticsearchHotelModel>(searchDescriptor);
        var list = response.Documents;

        var queryDebugInfo = response.DebugInformation; 

        return list.ToList();
    }
}
