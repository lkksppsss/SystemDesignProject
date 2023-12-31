﻿using SP.Hotel.Domain.AggregatesModel.HotelAggregate;

namespace SP.Hotel.API.Application.Models;

public class CreateHotelModel : ICreateHotel
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Area { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }

    public List<ICreateHotelPicture> Pictures { get; set; }
}
