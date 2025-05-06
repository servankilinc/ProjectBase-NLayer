using DataAccess.Abstract;

namespace DataAccess.UoW;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    IBlogRepository BlogRepository { get; }
    IBlogLikeMapRepository BlogLikeMapRepository { get; }
    IBlogCommentMapRepository BlogCommentMapRepository { get; }


    int SaveChanges();
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
