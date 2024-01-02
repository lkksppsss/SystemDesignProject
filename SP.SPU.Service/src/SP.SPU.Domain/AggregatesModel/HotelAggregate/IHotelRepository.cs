using SP.SPU.Domain.SeedWork;

namespace SP.SPU.Domain.AggregatesModel.HotelAggregate;

public interface IHotelRepository : IRepository<HotelEntity>
{
    void Add(HotelEntity installmentEntity);
    Task<HotelEntity> GetAsync(int id);
}
