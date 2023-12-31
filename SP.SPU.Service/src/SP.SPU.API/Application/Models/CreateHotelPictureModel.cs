﻿using SP.SPU.Domain.AggregatesModel.HotelAggregate;

namespace SP.SPU.API.Application.Models;

public class CreateHotelPictureModel : ICreateHotelPicture
{
    public HotelPicType Type { get; set; }
    public string Url { get; set; }
}
