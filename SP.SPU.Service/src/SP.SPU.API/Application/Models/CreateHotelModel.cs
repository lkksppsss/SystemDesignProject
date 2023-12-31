﻿using SP.SPU.Domain.AggregatesModel.HotelAggregate;

namespace SP.SPU.API.Application.Models;

public class CreateHotelModel : ICreateHotel
{
    public int HotelId { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Area { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }

    public List<ICreateHotelPicture> Pictures { get; set; }
}
