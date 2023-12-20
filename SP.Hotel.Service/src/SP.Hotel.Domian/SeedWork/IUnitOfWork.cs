namespace SP.Hotel.Domian.SeedWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
}
