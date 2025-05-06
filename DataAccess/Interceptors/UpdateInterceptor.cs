using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Core.Model;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Core.Enums;
using Newtonsoft.Json;
using System.Net;
using Model.ProjectEntities;

namespace Core.Utils.Repository.Interceptors;

public sealed class UpdateInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UpdateInterceptor(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        // 1) Auditable entity process handling
        IEnumerable<EntityEntry<IAuditableEntity>> auditableEntries = eventData.Context.ChangeTracker.Entries<IAuditableEntity>()
            .Where(e => e.State == EntityState.Modified && e.Entity is not IProjectEntity);
        
        if (auditableEntries.Count() > 0)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            foreach (EntityEntry<IAuditableEntity> entry in auditableEntries)
            {
                entry.Entity.UpdatedBy = userId ?? "Not found";
                entry.Entity.UpdateDateUtc = DateTime.UtcNow;
            }
        }

        // 2) Loggable entity process handling
        IEnumerable<EntityEntry<ILoggableEntity>> loggableEntries = eventData.Context.ChangeTracker.Entries<ILoggableEntity>()
            .Where(e => e.State == EntityState.Modified && e.Entity is not IProjectEntity);

        if (loggableEntries.Count() > 0)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? userAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent;
            IPAddress? ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress;

            foreach (EntityEntry<ILoggableEntity> entry in loggableEntries)
            {
                string changes = string.Empty;
                if(entry.Entity != null)
                {
                    changes = JsonConvert.SerializeObject(new
                    {
                        New = entry.OriginalValues.ToObject(),
                        Old = entry.CurrentValues.ToObject()
                    });
                }
                
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
                    Action = CrudTypes.Update,
                    DateUtc = DateTime.UtcNow,
                    Data = changes,
                });
            }
        }


        // 3) Archivable entity process handling
        IEnumerable<EntityEntry<IArchivableEntity>> archivableEntries = eventData.Context.ChangeTracker.Entries<IArchivableEntity>()
            .Where(e => e.State == EntityState.Modified && e.Entity is not IProjectEntity);

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
                    Action = CrudTypes.Update,
                    DateUtc = DateTime.UtcNow,
                    Data = entry.OriginalValues != null ? JsonConvert.SerializeObject(entry.OriginalValues.ToObject()) : default,
                });
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}