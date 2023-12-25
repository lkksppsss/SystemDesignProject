using AutoMapper;
using MediatR;
using SP.SPU.API.Application.Models;
using SP.SPU.Domian.AggregatesModel.HotelAggregate;

namespace SP.SPU.API.Application.Commands;

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
