using Nest;

namespace SP.SPU.Infrastructure.ElasticSearch.Models;

public class ElasticsearchHotelModel
{
    [Number(Name = "hotel_id", Index = false)]
    public int HotelId { get; set; }

    [Keyword(Name = "hotel_name", Index = true)]
    public string Name { get; set; }

    [Keyword(Name = "city", Index = true)]
    public string City { get; set; }

    [Keyword(Name = "area", Index = true)]
    public string Area { get; set; }

    [Keyword(Name = "address", Index = true)]
    public string Address { get; set; }

    [Text(Name = "description", Index = true)]
    public string Description { get; set; }

    [Object]
    [PropertyName("hotel_pictures")]
    public List<ElasticsearchHotelPiceture> Pictures { get; set; }
}

public class ElasticsearchHotelPiceture
{
    /// <summary>
    /// 相片類型
    /// 0: 預設
    /// 1: 房間
    /// 2: 設施
    /// 3: 建築物
    /// </summary>
    [Number(Name = "type", Index = false)]
    public int Type { get; set; }

    [Text(Name = "url", Index = false)]
    public string Url { get; set; }
}
