using MediatR;
using Microsoft.AspNetCore.Mvc;
using SP.Hotel.API.Application.Commands;
using SP.Hotel.API.Application.IntegrationEvents.PublishEvents;
using SP.Hotel.API.SeedWork;
using SPCorePackage.Kafka.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace SP.Hotel.API.Controllers;

[ApiController]
public class HotelController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IEventBus _eventBus;

    public HotelController(IMediator mediator, IEventBus eventBus)
    {
        _mediator = mediator;
        _eventBus = eventBus;
    }

    [HttpPost("/backend/1.0/hotel/create_hotel")]
    [SwaggerOperation(Summary = "新增飯店", Tags = new[] { "後台-飯店管理" })]
    [ProducesResponseType(typeof(SPResponse<bool>), 200)]
    public async Task<IActionResult> GetProductInstallment([FromBody] CreateHotelCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new SPResponse<bool>(result));
    }

    [HttpGet("/backend/1.0/hotel/kafka_test")]
    [SwaggerOperation(Summary = "新增飯店", Tags = new[] { "後台-飯店管理" })]
    [ProducesResponseType(typeof(SPResponse<bool>), 200)]
    public async Task<IActionResult> TestKafka()
    {
        var pubEvent = new TestPublishEvent()
        {
            Name = "Test",
            Number = 1,
            Time = DateTime.Now,
            IsBool = true,
        };

        await _eventBus.PublishAsync(TestPublishEvent.EventName, pubEvent);
        return Ok(new SPResponse<bool>(true));
    }
}
