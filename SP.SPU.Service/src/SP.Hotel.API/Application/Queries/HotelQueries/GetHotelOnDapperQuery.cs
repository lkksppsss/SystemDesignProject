using MediatR;
using SP.SPU.API.ViewModels;

namespace SP.SPU.API.Application.Queries.HotelQueries;

public class GetHotelOnDapperQuery : IRequest<HotelVo>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Area { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
}
