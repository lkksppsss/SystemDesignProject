using MediatR;
using Microsoft.AspNetCore.Mvc;
using SP.SPU.API.Application.Commands;
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

    [HttpGet("/app/1.0/spu/get_hotel_on_dapper")]
    [SwaggerOperation(Summary = "取得飯店資訊-dapper", Tags = new[] { "前台-飯店管理" })]
    [ProducesResponseType(typeof(SPResponse<HotelVo>), 200)]
    public async Task<IActionResult> GetHotelOnDappe([FromQuery]GetHotelOnDapperQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(new SPResponse<HotelVo>(result));
    }

    [HttpGet("/app/1.0/spu/get_hotel_on_elastic")]
    [SwaggerOperation(Summary = "取得飯店資訊-elastic", Tags = new[] { "前台-飯店管理" })]
    [ProducesResponseType(typeof(SPResponse<List<HotelVo>>), 200)]
    public async Task<IActionResult> GetHotelOnElastic([FromQuery] GetHotelOnElasticQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(new SPResponse<List<HotelVo>>(result));
    }

    [HttpPost("/backdoor/1.0/spu/add_data_to_elastic")]
    [SwaggerOperation(Summary = "新增飯店資訊to-elastic", Tags = new[] { "後門-飯店管理" })]
    [ProducesResponseType(typeof(SPResponse<bool>), 200)]
    public async Task<IActionResult> AddDataToElastic([FromBody] AddDataToElasticCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new SPResponse<bool>(result));
    }
}
