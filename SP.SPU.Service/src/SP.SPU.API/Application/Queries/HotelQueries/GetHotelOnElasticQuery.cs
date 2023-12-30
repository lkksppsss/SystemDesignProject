using MediatR;
using SP.SPU.API.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace SP.SPU.API.Application.Queries.HotelQueries;

public class GetHotelOnElasticQuery : IRequest<List<HotelVo>>
{
    /// <summary>
    /// 關鍵字
    /// </summary>
    [MinLength(1)]
    [MaxLength(50)]
    [Required]
    public string Keyword { get; set; }
}
