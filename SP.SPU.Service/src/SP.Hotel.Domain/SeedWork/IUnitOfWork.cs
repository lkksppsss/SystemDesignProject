namespace SP.SPU.Domain.SeedWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
}
