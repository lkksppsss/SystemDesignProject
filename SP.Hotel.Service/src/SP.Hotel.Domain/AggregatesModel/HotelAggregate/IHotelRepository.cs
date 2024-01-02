using SP.Hotel.Domain.SeedWork;

namespace SP.Hotel.Domain.AggregatesModel.HotelAggregate;

public interface IHotelRepository : IRepository<HotelEntity>
{
    void Add(HotelEntity installmentEntity);
}
