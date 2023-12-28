using Microsoft.EntityFrameworkCore;
using SP.SPU.Domian.AggregatesModel.HotelAggregate;
using SP.SPU.Domian.SeedWork;

namespace SP.SPU.Infrastructure.Repositories;

public class HotelRepository : IHotelRepository
{
    protected readonly DataContext _context;
    public HotelRepository(DataContext dataContext)
    {
        _context = dataContext;
    }

    public IUnitOfWork UnitOfWork => _context;

    public void Add(HotelEntity installmentEntity)
    {
        _context.Hotels.Add(installmentEntity);
    }

    public async Task<HotelEntity> GetAsync(int id)
    {
        return await _context.Hotels.Include(x => x.Pictures ).FirstOrDefaultAsync(x => x.Id == id);
    }
}
