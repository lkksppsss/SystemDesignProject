using MediatR;
using SP.SPU.API.ViewModels;

namespace SP.SPU.API.Application.Queries.HotelQueries;

public class GetHotelOnDapperQueryHandler : IRequestHandler<GetHotelOnDapperQuery, HotelVo>
{
    public Task<HotelVo> Handle(GetHotelOnDapperQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
