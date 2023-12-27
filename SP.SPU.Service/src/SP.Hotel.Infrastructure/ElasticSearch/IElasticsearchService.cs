using SP.SPU.Infrastructure.ElasticSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.SPU.Infrastructure.ElasticSearch;

public interface IElasticsearchService
{
    void Insert(ElasticsearchHotelModel houseDTO);
    List<ElasticsearchHotelModel> Search(ElasticsearchSearchRequest request);
}
