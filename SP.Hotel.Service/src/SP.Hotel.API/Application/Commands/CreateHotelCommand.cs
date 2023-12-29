﻿using MediatR;
using SP.Hotel.Domain.AggregatesModel.HotelAggregate;

namespace SP.Hotel.API.Application.Commands;

public class CreateHotelCommand : IRequest<bool>
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Area { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }

    public List<CreateHotelPicture> Pictures { get; set; } = new();
}

public class CreateHotelPicture
{
    public int Type { get; set; }
    public string Url { get; set; }
}
