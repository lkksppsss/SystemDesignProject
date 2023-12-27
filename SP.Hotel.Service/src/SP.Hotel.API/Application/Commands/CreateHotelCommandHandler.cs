using AutoMapper;
using MediatR;
using SP.Hotel.API.Application.IntegrationEvents.PublishEvents;
using SP.Hotel.API.Application.Models;
using SP.Hotel.Domian.AggregatesModel.HotelAggregate;
using SPCorePackage.Kafka.Interface;

namespace SP.Hotel.API.Application.Commands;

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, bool>
{
    private readonly IHotelRepository _hotelRepo;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public CreateHotelCommandHandler(IHotelRepository hotelRepo, IMapper mapper, IEventBus eventBus)
    {
        _hotelRepo = hotelRepo;
        _mapper = mapper;
        _eventBus = eventBus;
    }
    public async Task<bool> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotelModel = _mapper.Map<CreateHotelModel>(request);
        var hotelEntity = new HotelEntity(hotelModel);
        _hotelRepo.Add(hotelEntity);

        bool result = await _hotelRepo.UnitOfWork.SaveEntitiesAsync() > 0;
        if (result)
        {
            var createEvent = new CreateHotelEvent(hotelEntity);
            await _eventBus.PublishAsync(CreateHotelEvent.EventName, createEvent);
        }
        return result;
    }
}
