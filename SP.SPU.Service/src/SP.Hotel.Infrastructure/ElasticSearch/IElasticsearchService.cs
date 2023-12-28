using SP.SPU.Infrastructure.ElasticSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.SPU.Infrastructure.ElasticSearch;

public interface IElasticsearchService
{
    Task Insert(ElasticsearchHotelModel houseDTO);
    Task<List<ElasticsearchHotelModel>> SearchAsync(ElasticsearchSearchRequest request);
    Task CreateIndex();
}
