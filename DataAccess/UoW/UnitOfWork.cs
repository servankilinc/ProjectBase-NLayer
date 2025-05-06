using DataAccess.Abstract;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private IDbContextTransaction _transaction;

    #region UserRepository
    private IUserRepository _userRepository;
    public IUserRepository UserRepository => _userRepository ??= _serviceProvider.GetRequiredService<IUserRepository>();
    #endregion

    #region BlogRepository
    private IBlogRepository _blogRepository;
    public IBlogRepository BlogRepository => _blogRepository ??= _serviceProvider.GetRequiredService<IBlogRepository>();
    #endregion

    #region BlogLikeMapRepository
    private IBlogLikeMapRepository _blogLikeMapRepository;
    public IBlogLikeMapRepository BlogLikeMapRepository => _blogLikeMapRepository ??= _serviceProvider.GetRequiredService<IBlogLikeMapRepository>();
    #endregion

    #region BlogCommentMapRepository
    private IBlogCommentMapRepository _blogCommentMapRepository;
    public IBlogCommentMapRepository BlogCommentMapRepository => _blogCommentMapRepository ??= _serviceProvider.GetRequiredService<IBlogCommentMapRepository>();
    #endregion

    public UnitOfWork(AppDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }


    #region Sync Methods
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }
    public void BeginTransaction()
    {
        if (_transaction != null) throw new InvalidOperationException("Transaction already started.");

        _transaction = _context.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        if (_transaction == null) throw new InvalidOperationException("Transaction has not been started.");

        _transaction.Commit();
        _transaction.Dispose();
        _transaction = null;
    }

    public void RollbackTransaction()
    {
        if (_transaction != null) 
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }
    }
    #endregion


    #region Async Methods
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null) throw new InvalidOperationException("Transaction already started.");

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null) throw new InvalidOperationException("Transaction has not been started.");
        
        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null) 
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
    #endregion

    public void Dispose()
    {
        _context.Dispose();
        if (_transaction != null)
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }
}