namespace SP.SPU.API.Application.Dto;

public class HotelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Area { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }

    public List<HotelPicetureDto> Pictures { get; set; } = new();
}
public class HotelPicetureDto
{
    public int Type { get; set; }
    public string Url { get; set; }
}

