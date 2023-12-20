using AutoMapper;
using MediatR;
using SP.Hotel.API.Application.Models;
using SP.Hotel.Domian.AggregatesModel.HotelAggregate;

namespace SP.Hotel.API.Application.Commands;

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, bool>
{
    private readonly IHotelRepository _hotelRepo;
    private readonly IMapper _mapper;

    public CreateHotelCommandHandler(IHotelRepository hotelRepo, IMapper mapper)
    {
        _hotelRepo = hotelRepo;
        _mapper = mapper;
    }
    public async Task<bool> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotelModel = _mapper.Map<CreateHotelModel>(request);
        var hotelEntity = new HotelEntity(hotelModel);
        _hotelRepo.Add(hotelEntity);

        return await _hotelRepo.UnitOfWork.SaveEntitiesAsync() > 0;
    }
}
