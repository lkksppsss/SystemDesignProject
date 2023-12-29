using SP.Hotel.Domain.AggregatesModel.HotelAggregate;
using SP.Hotel.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Hotel.Infrastructure.Repositories;

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
}
