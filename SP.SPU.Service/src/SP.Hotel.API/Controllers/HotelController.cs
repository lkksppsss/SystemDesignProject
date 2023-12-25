using MediatR;
using Microsoft.AspNetCore.Mvc;
using SP.SPU.API.Application.Queries.HotelQueries;
using SP.SPU.API.ViewModels;
using SPCorePackage.Kafka.Interface;
using SPCorePackage.SeedWork;
using Swashbuckle.AspNetCore.Annotations;

namespace SP.SPU.API.Controllers;

[ApiController]
public class HotelController : ControllerBase
{
    private readonly IMediator _mediator;

    public HotelController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/app/1.0/spu/get_hotel_on_dapper")]
    [SwaggerOperation(Summary = "取得飯店資訊-dapper", Tags = new[] { "前台-飯店管理" })]
    [ProducesResponseType(typeof(SPResponse<HotelVo>), 200)]
    public async Task<IActionResult> GetProductInstallment([FromQuery]GetHotelOnDapperQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(new SPResponse<HotelVo>(result));
    }
}
