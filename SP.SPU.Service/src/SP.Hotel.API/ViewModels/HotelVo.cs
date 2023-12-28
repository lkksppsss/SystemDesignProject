using Nest;
using SP.SPU.API.Application.IntegrationEvents.PublishEvents;
using SP.SPU.Infrastructure.ElasticSearch.Models;

namespace SP.SPU.API.ViewModels;

public class HotelVo
{
    public string Name { get; private set; }
    public string City { get; private set; }
    public string Area { get; private set; }
    public string Address { get; private set; }
    public string Description { get; private set; }

    public List<HotelPicetureVo> Pictures { get; set; }
}
public class HotelPicetureVo
{
    public int Type { get; set; }
    public string Url { get; set; }
}
