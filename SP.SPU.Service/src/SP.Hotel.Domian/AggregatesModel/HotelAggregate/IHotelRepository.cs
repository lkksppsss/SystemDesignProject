using SP.SPU.Domian.SeedWork;

namespace SP.SPU.Domian.AggregatesModel.HotelAggregate;

public interface IHotelRepository : IRepository<HotelEntity>
{
    void Add(HotelEntity installmentEntity);
    Task<HotelEntity> GetAsync(int id);
}
