using Core.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Core.Utils.HttpContextManager;

namespace DataAccess.Interceptors;

public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    private readonly HttpContextManager _httpContextManager;
    public SoftDeleteInterceptor(HttpContextManager httpContextManager) => _httpContextManager = httpContextManager;


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
                entry.Entity.DeletedBy = _httpContextManager.GetUserId();
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
                entry.Entity.DeletedBy = _httpContextManager.GetUserId();
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedDateUtc = DateTime.UtcNow;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
