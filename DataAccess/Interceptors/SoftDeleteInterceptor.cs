using Core.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Core.Utils.RequestInfoProvider;

namespace DataAccess.Interceptors;

public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    private readonly RequestInfoProvider _requestInfoProvider;
    public SoftDeleteInterceptor(RequestInfoProvider requestInfoProvider) => _requestInfoProvider = requestInfoProvider;


    //  ****************************** SYNC VERSION ******************************
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null) return base.SavingChanges(eventData, result);

        IEnumerable<EntityEntry<ISoftDeletableEntity>> softDeletableEntries = eventData.Context.ChangeTracker.Entries<ISoftDeletableEntity>()
            .Where(e => e.State == EntityState.Deleted && e.Entity is not IProjectEntity);

        if (softDeletableEntries.Any())
        {
            foreach (EntityEntry<ISoftDeletableEntity> entry in softDeletableEntries)
            {
                entry.State = EntityState.Modified;
                entry.Entity.DeletedBy = _requestInfoProvider.GetUserId();
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedDateUtc = DateTime.UtcNow;
            }
        }

        return base.SavingChanges(eventData, result);
    }


    //  ****************************** ASYNC VERSION ******************************
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        IEnumerable<EntityEntry<ISoftDeletableEntity>> softDeletableEntries = eventData.Context.ChangeTracker.Entries<ISoftDeletableEntity>()
            .Where(e => e.State == EntityState.Deleted && e.Entity is not IProjectEntity);

        if (softDeletableEntries.Any())
        {
            foreach (EntityEntry<ISoftDeletableEntity> entry in softDeletableEntries)
            {
                entry.State = EntityState.Modified;
                entry.Entity.DeletedBy = _requestInfoProvider.GetUserId();
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedDateUtc = DateTime.UtcNow;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
