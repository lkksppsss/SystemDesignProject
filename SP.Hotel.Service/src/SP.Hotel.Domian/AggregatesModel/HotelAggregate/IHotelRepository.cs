using SP.Hotel.Domian.SeedWork;

namespace SP.Hotel.Domian.AggregatesModel.HotelAggregate;

public interface IHotelRepository : IRepository<HotelEntity>
{
    void Add(HotelEntity installmentEntity);
}
