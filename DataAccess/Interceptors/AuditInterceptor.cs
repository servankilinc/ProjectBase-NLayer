using Core.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Core.Utils.RequestInfoProvider;

namespace DataAccess.Interceptors;

public sealed class AuditInterceptor : SaveChangesInterceptor
{
    private readonly RequestInfoProvider _requestInfoProvider;
    public AuditInterceptor(RequestInfoProvider requestInfoProvider) => _requestInfoProvider = requestInfoProvider;


    //  ****************************** SYNC VERSION ******************************
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null) return base.SavingChanges(eventData, result);

        IEnumerable<EntityEntry<IAuditableEntity>> auditableEntries = eventData.Context.ChangeTracker.Entries<IAuditableEntity>()
            .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified) && e.Entity is not IProjectEntity);

        if (auditableEntries.Any())
        {
            foreach (EntityEntry<IAuditableEntity> entry in auditableEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _requestInfoProvider.GetUserId();
                    entry.Entity.CreateDateUtc = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedBy = _requestInfoProvider.GetUserId();
                    entry.Entity.UpdateDateUtc = DateTime.UtcNow;
                }
            }
        }

        return base.SavingChanges(eventData, result);
    }


    //  ****************************** ASYNC VERSION ******************************
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        IEnumerable<EntityEntry<IAuditableEntity>> auditableEntries = eventData.Context.ChangeTracker.Entries<IAuditableEntity>()
            .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified) && e.Entity is not IProjectEntity);

        if (auditableEntries.Any())
        {
            foreach (EntityEntry<IAuditableEntity> entry in auditableEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _requestInfoProvider.GetUserId();
                    entry.Entity.CreateDateUtc = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedBy = _requestInfoProvider.GetUserId();
                    entry.Entity.UpdateDateUtc = DateTime.UtcNow;
                }
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
