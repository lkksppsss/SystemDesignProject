using AutoMapper;
using Dapper;
using MediatR;
using SP.SPU.API.Application.Dto;
using SP.SPU.API.ViewModels;
using SP.SPU.Infrastructure.SeedWork;
using System.Text;

namespace SP.SPU.API.Application.Queries.HotelQueries;

public class GetHotelOnDapperQueryHandler : IRequestHandler<GetHotelOnDapperQuery, List<HotelVo>>
{
    private readonly IUnitOfWorkDapper _dapper;
    private readonly IMapper  _mapper;

    public GetHotelOnDapperQueryHandler(IUnitOfWorkDapper dapper, IMapper mapper)
    {
        _dapper = dapper;
        _mapper = mapper;
    }

    public async Task<List<HotelVo>> Handle(GetHotelOnDapperQuery request, CancellationToken cancellationToken)
    {
        DynamicParameters parameters = new();
        List<string> condition = new List<string>();
        if (request.Id > 0)
        {
            parameters.Add("@Id", request.Id);
            condition.Add(" h.id = @Id ");
        }
        if (!string.IsNullOrEmpty(request.Name))
        {
            parameters.Add("@Name", request.Name);
            condition.Add(" h.name = @Name ");
        }
        if (!string.IsNullOrEmpty(request.City))
        {
            parameters.Add("@City", request.City);
            condition.Add(" h.city = @City ");
        }
        if (!string.IsNullOrEmpty(request.Area))
        {
            parameters.Add("@Area", request.Area);
            condition.Add(" h.area = @Area ");
        }
        if (!string.IsNullOrEmpty(request.Address))
        {
            parameters.Add("@Address", request.Address);
            condition.Add(" h.address = @Address ");
        }
        StringBuilder conditionSql = new();
        if (condition.Any())
        {
            conditionSql.Append(" Where ");
            conditionSql.AppendJoin(" And ", condition);
        }

        string sql = @"
            Select
                h.id,
                h.name,
                h.city, 
                h.area, 
                h.address, 
                h.description,
                hp.type,
                hp.url
            From hotels h
            Inner Join hotel_pictures hp On h.id = hp.hotel_id
        " + conditionSql.ToString();
        var dataDic = new Dictionary<int, HotelDto>();
        var query = await _dapper.Slave.QueryAsync<HotelDto, HotelPicetureDto, HotelDto>(sql,
            (main, child) =>
            {
                if (!dataDic.TryGetValue(main.Id, out HotelDto dto))
                {
                    dto = main;
                    dataDic.Add(main.Id, main);
                }
                dto.Pictures.Add(child);
                return main;
            }
            , param: parameters, splitOn: "type");

        var result = _mapper.Map<List<HotelVo>>(dataDic.Values.ToList());

        return result;
    }
}
