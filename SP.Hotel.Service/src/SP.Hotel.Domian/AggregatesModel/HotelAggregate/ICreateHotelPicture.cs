using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Hotel.Domian.AggregatesModel.HotelAggregate;

public interface ICreateHotelPicture
{
    public HotelPicType Type { get; set; }
    public string Url { get; set; }
}
