using MediatR;

namespace SP.SPU.API.Application.Commands;

public class AddDataToElasticCommand : IRequest<bool>
{
    public int Id { get; set; }
}
