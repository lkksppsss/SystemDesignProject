namespace SP.SPU.API.ViewModels;

public class HotelVo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Area { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }

    public List<HotelPicetureVo> Pictures { get; set; }
}
public class HotelPicetureVo
{
    public int Type { get; set; }
    public string Url { get; set; }
}
