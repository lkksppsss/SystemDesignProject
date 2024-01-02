using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using SP.SPU.Domian.AggregatesModel.HotelAggregate;
using SP.SPU.Domian.SeedWork;
using SP.SPU.Infrastructure.EntityConfig;
using System.Data;

namespace SP.SPU.Infrastructure;


public class DataContext : DbContext, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "spu";

    private readonly ILogger<DataContext> _logger;
    private readonly IMediator _mediator;
    private IDbContextTransaction _currentTransaction;

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    public DataContext(DbContextOptions<DataContext> options, IMediator mediator, ILogger<DataContext> logger) : base(options)
    {
        _logger = logger;
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        System.Diagnostics.Debug.WriteLine("DataContext::ctor ->" + this.GetHashCode());
    }

    public DbSet<HotelEntity> Hotels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new HotelEntityConfig());
        modelBuilder.ApplyConfiguration(new HotelPictureEntityConfig());
    }
    public async Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using (var transaction = await BeginTransactionAsync())
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                await CommitTransactionAsync(transaction);
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            transaction.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}