using Core.Model;
using Core.Utils.HttpContextManager;
using DataAccess.Interceptors.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Model.ProjectEntities;

namespace DataAccess.Interceptors;

public sealed class LogInterceptor : SaveChangesInterceptor
{
    private readonly HttpContextManager _httpContextManager;
    private readonly List<(Log LogEntry, EntityEntry EntityEntry)> _pendingLogs = new();
    public LogInterceptor(HttpContextManager httpContextManager) => _httpContextManager = httpContextManager;


    //  ****************************** SYNC VERSION ******************************
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null) return base.SavingChanges(eventData, result);

        IEnumerable<EntityEntry<ILoggableEntity>> loggableEntries = eventData.Context.ChangeTracker.Entries<ILoggableEntity>()
            .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted) && e.Entity is not IProjectEntity);

        if (loggableEntries.Any())
        {
            List<Log> logsToInsert = new List<Log>();
            foreach (EntityEntry<ILoggableEntity> entry in loggableEntries)
            {
                Log log = new Log
                {
                    TableName = entry.GetTableName(),
                    RequesterId = _httpContextManager.GetUserId(),
                    ClientIp = _httpContextManager.GetClientIp(),
                    UserAgent = _httpContextManager.GetUserAgent(),
                    Action = entry.GetActionType(),
                    DateUtc = DateTime.UtcNow,
                };

                if (entry.State == EntityState.Added)
                {
                    _pendingLogs.Add((log, entry));
                }
                else if (entry.State == EntityState.Deleted)
                {
                    log.EntityId = entry.GetEntityId();
                    log.Data = entry.GetOriginalData();

                    logsToInsert.Add(log);
                }
                else if (entry.State == EntityState.Modified)
                {
                    log.EntityId = entry.GetEntityId();
                    log.OldData = entry.GetOriginalData();
                    log.NewData = entry.GetCurrentData();

                    logsToInsert.Add(log);
                }
            }
            eventData.Context.Set<Log>().AddRange(logsToInsert);
        }

        return base.SavingChanges(eventData, result);
    }


    //  ****************************** ASYNC VERSION ******************************
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        IEnumerable<EntityEntry<ILoggableEntity>> loggableEntries = eventData.Context.ChangeTracker.Entries<ILoggableEntity>()
            .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted) && e.Entity is not IProjectEntity);

        if (loggableEntries.Any())
        {
            List<Log> logsToInsert = new List<Log>();
            foreach (EntityEntry<ILoggableEntity> entry in loggableEntries)
            {
                Log log = new Log
                {
                    TableName = entry.GetTableName(),
                    RequesterId = _httpContextManager.GetUserId(),
                    ClientIp = _httpContextManager.GetClientIp(),
                    UserAgent = _httpContextManager.GetUserAgent(),
                    Action = entry.GetActionType(),
                    DateUtc = DateTime.UtcNow,
                };

                if (entry.State == EntityState.Added)
                {
                    _pendingLogs.Add((log, entry));
                }
                else if (entry.State == EntityState.Deleted)
                {
                    log.EntityId = entry.GetEntityId();
                    log.Data = entry.GetOriginalData();

                    logsToInsert.Add(log);
                }
                else if (entry.State == EntityState.Modified)
                {
                    log.EntityId = entry.GetEntityId();
                    log.OldData = entry.GetOriginalData();
                    log.NewData = entry.GetCurrentData();

                    logsToInsert.Add(log);
                }
            }
            eventData.Context.Set<Log>().AddRange(logsToInsert);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }




    //  ****************************** SYNC VERSION ******************************
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        if (eventData.Context is null) return base.SavedChanges(eventData, result);

        if (_pendingLogs.Any())
        {
            foreach (var (log, entityEntry) in _pendingLogs)
            {
                log.EntityId = entityEntry.GetEntityId();
                log.Data = entityEntry.GetCurrentData();
            }

            eventData.Context.ChangeTracker.AutoDetectChangesEnabled = false;
            eventData.Context.Set<Log>().AddRange(_pendingLogs.Select(p => p.LogEntry));
            eventData.Context.SaveChanges();
            eventData.Context.ChangeTracker.AutoDetectChangesEnabled = true;

            _pendingLogs.Clear();
        }

        return base.SavedChanges(eventData, result);
    }

    //  ****************************** ASYNC VERSION ******************************
    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return base.SavedChangesAsync(eventData, result, cancellationToken);

        if (_pendingLogs.Any())
        {
            foreach (var (log, entityEntry) in _pendingLogs)
            {
                log.EntityId = entityEntry.GetEntityId();
                log.Data = entityEntry.GetCurrentData();
            }

            eventData.Context.ChangeTracker.AutoDetectChangesEnabled = false;
            eventData.Context.Set<Log>().AddRange(_pendingLogs.Select(p => p.LogEntry));
            _pendingLogs.Clear();
            eventData.Context.SaveChanges();
            eventData.Context.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
