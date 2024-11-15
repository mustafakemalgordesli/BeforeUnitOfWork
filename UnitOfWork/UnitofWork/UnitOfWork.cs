using UnitOfWork.Abstracts;
using UnitOfWork.Entities;
using UnitOfWork.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace UnitOfWork.UnitofWork;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    protected readonly AppDbContext _context;
    private readonly IUserRepository _userRepository;
    public UnitOfWork(AppDbContext context, IUserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }

    public IUserRepository UserRepository => _userRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public virtual void Dispose()
    {
        _context.Dispose();
    }
}

public class TransactionUnitOfWork : UnitOfWork, IDisposable, ITransactionUnitOfWork
{
    private IDbContextTransaction _transaction;

    public TransactionUnitOfWork(AppDbContext context,
                                 IUserRepository userRepository) : base(context, userRepository)
    {
    }

    public override void Dispose()
    {
        _context.Dispose();
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            if (_transaction != null)
            {
                await RollbackAsync(cancellationToken);
            }
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}