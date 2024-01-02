using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Hotel.Domian.AggregatesModel.HotelAggregate;

public interface ICreateHotel
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Area { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }

    public List<ICreateHotelPicture> Pictures { get; set; }
}
