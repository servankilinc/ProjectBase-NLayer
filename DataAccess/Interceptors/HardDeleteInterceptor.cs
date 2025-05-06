using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Core.Model;
using Core.Enums;
using Model.ProjectEntities;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Core.Utils.Repository.Interceptors;

public sealed class HardDeleteInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public HardDeleteInterceptor(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;


    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);
 
        // 1) Loggable entity process handling
        IEnumerable<EntityEntry<ILoggableEntity>> loggableEntries = eventData.Context.ChangeTracker.Entries<ILoggableEntity>()
            .Where(e => e.State == EntityState.Deleted && e.Entity is not ISoftDeletableEntity && e.Entity is not IProjectEntity);

        if (loggableEntries.Count() > 0)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? userAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent;
            IPAddress? ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress;

            foreach (EntityEntry<ILoggableEntity> entry in loggableEntries)
            {
                var primaryKeys = entry.Metadata.FindPrimaryKey()?.Properties.Select(pk => entry.Property(pk.Name).CurrentValue?.ToString()).Where(v => !string.IsNullOrEmpty(v));
                string? entityId = default;

                if (primaryKeys != null && primaryKeys.Count() > 0)
                {
                    if (primaryKeys.Count() == 1) entityId = primaryKeys.FirstOrDefault();
                    else entityId = primaryKeys.OrderByDescending(x => x).Aggregate((a, b) => $"{a}-{b}");
                }

                eventData.Context.Set<ProjectLog>().Add(new ProjectLog
                {
                    TableName = entry.Entity?.GetType().Name,
                    EntityId = entityId,
                    RequesterId = userId,
                    ClientIp = ipAddress?.ToString(),
                    UserAgent = userAgent?.ToString(),
                    DateUtc = DateTime.UtcNow,
                    Action = CrudTypes.Delete,
                    Data = entry.Entity != null ? JsonConvert.SerializeObject(entry.OriginalValues.ToObject()) : default
                });
            }
        }

        // 3) Archivable entity process handling
        IEnumerable<EntityEntry<IArchivableEntity>> archivableEntries = eventData.Context.ChangeTracker.Entries<IArchivableEntity>()
            .Where(e => e.State == EntityState.Deleted && e.Entity is not ISoftDeletableEntity && e.Entity is not IProjectEntity);

        if (loggableEntries.Count() > 0)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? userAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent;
            IPAddress? ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress;

            foreach (EntityEntry<ILoggableEntity> entry in loggableEntries)
            {
                var primaryKeys = entry.Metadata.FindPrimaryKey()?.Properties.Select(pk => entry.Property(pk.Name).CurrentValue?.ToString()).Where(v => !string.IsNullOrEmpty(v));
                string? entityId = default;

                if (primaryKeys != null && primaryKeys.Count() > 0)
                {
                    if (primaryKeys.Count() == 1) entityId = primaryKeys.FirstOrDefault();
                    else entityId = primaryKeys.OrderByDescending(x => x).Aggregate((a, b) => $"{a}-{b}");
                }

                eventData.Context.Set<ProjectArchive>().Add(new ProjectArchive
                {
                    TableName = entry.Entity != null ? entry.Entity.GetType().Name : default,
                    EntityId = entityId,
                    RequesterId = userId,
                    ClientIp = ipAddress?.ToString(),
                    UserAgent = userAgent?.ToString(),
                    Action = CrudTypes.Delete,
                    DateUtc = DateTime.UtcNow,
                    Data = entry.OriginalValues != null ? JsonConvert.SerializeObject(entry.OriginalValues.ToObject()) : default,
                });
            }
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}