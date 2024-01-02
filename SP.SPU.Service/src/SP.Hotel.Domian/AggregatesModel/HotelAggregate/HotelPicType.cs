using SP.SPU.Domian.SeedWork;

namespace SP.SPU.Domian.AggregatesModel.HotelAggregate;

public class HotelPicType : Enumeration
{
    public static HotelPicType Default = new HotelPicType(0, "預設");
    public static HotelPicType Room = new HotelPicType(1, "房間");
    public static HotelPicType Facility = new HotelPicType(2, "設施");
    public static HotelPicType Building = new HotelPicType(3, "建築物");

    public HotelPicType(int id, string name) : base(id, name)
    {
    }
    public static IEnumerable<HotelPicType> List() =>
        new[] { Default, Room, Facility, Building };

    public static HotelPicType From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);
        if (state == null)
        {
            return null;
        }

        return state;
    }
}
