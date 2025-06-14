using Core.Model;
using Core.Utils.HttpContextManager;
using DataAccess.Interceptors.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Model.ProjectEntities;

namespace DataAccess.Interceptors;

public sealed class ArchiveInterceptor : SaveChangesInterceptor
{
    private readonly HttpContextManager _httpContextManager;
    public ArchiveInterceptor(HttpContextManager httpContextManager) => _httpContextManager = httpContextManager;


    //  ****************************** SYNC VERSION ******************************
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null) return base.SavingChanges(eventData, result);

        IEnumerable<EntityEntry<IArchivableEntity>> archivableEntries = eventData.Context.ChangeTracker.Entries<IArchivableEntity>()
            .Where(e => (e.State == EntityState.Modified || e.State == EntityState.Deleted) && e.Entity is not IProjectEntity);

        if (archivableEntries.Any())
        {
            List<Archive> archives = new List<Archive>();
            foreach (EntityEntry<IArchivableEntity> entry in archivableEntries)
            {
                archives.Add(new Archive
                {
                    TableName = entry.GetTableName(),
                    EntityId = entry.GetEntityId(),
                    RequesterId = _httpContextManager.GetUserId(),
                    ClientIp = _httpContextManager.GetClientIp(),
                    UserAgent = _httpContextManager.GetUserAgent(),
                    Action = entry.GetActionType(),
                    DateUtc = DateTime.UtcNow,
                    Data = entry.GetOriginalData(),
                });
            }
            eventData.Context.Set<Archive>().AddRange(archives);
        }

        return base.SavingChanges(eventData, result);
    }


    //  ****************************** ASYNC VERSION ******************************
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        IEnumerable<EntityEntry<IArchivableEntity>> archivableEntries = eventData.Context.ChangeTracker.Entries<IArchivableEntity>()
            .Where(e => (e.State == EntityState.Modified || e.State == EntityState.Deleted) && e.Entity is not IProjectEntity);

        if (archivableEntries.Any())
        {
            List<Archive> archives = new List<Archive>();
            foreach (EntityEntry<IArchivableEntity> entry in archivableEntries)
            {
                archives.Add(new Archive
                {
                    TableName = entry.GetTableName(),
                    EntityId = entry.GetEntityId(),
                    RequesterId = _httpContextManager.GetUserId(),
                    ClientIp = _httpContextManager.GetClientIp(),
                    UserAgent = _httpContextManager.GetUserAgent(),
                    Action = entry.GetActionType(),
                    DateUtc = DateTime.UtcNow,
                    Data = entry.GetOriginalData(),
                });
            }
            eventData.Context.Set<Archive>().AddRange(archives);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
