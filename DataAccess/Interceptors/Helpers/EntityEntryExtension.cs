using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace DataAccess.Interceptors.Helpers;

public static class EntityEntryExtension
{
    public static string? GetTableName(this EntityEntry entry)
    {
        string? tableName = default;
        if (entry.Entity != null)
        {
            tableName = entry.Entity.GetType().Name;
        }
        return tableName;
    }

    public static string? GetEntityId(this EntityEntry entry)
    {
        var primaryKeys = entry.Metadata.FindPrimaryKey()?.Properties.Select(pk => entry.Property(pk.Name).CurrentValue?.ToString()).Where(v => !string.IsNullOrEmpty(v));
        string? entityId = default;
        if (primaryKeys != null && primaryKeys.Count() > 0)
        {
            if (primaryKeys.Count() == 1) entityId = primaryKeys.FirstOrDefault();
            else entityId = primaryKeys.OrderByDescending(x => x).Aggregate((a, b) => $"{a}-{b}");
        }
        return entityId;
    }

    public static CrudTypes GetActionType(this EntityEntry entry)
    {
        CrudTypes actionType = CrudTypes.Undefined;
        if (entry.State == EntityState.Added)
        {
            actionType = CrudTypes.Create;
        }
        else if (entry.State == EntityState.Modified)
        {
            actionType = CrudTypes.Update;
        }
        else if (entry.State == EntityState.Deleted)
        {
            actionType = CrudTypes.Delete;
        }
        return actionType;
    }

    public static string? GetOriginalData(this EntityEntry entry)
    {
        string? data = string.Empty;
        if (entry.OriginalValues != null)
        {
            data = JsonConvert.SerializeObject(entry.OriginalValues.ToObject(), new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                MaxDepth = 7,
            });
        }
        return data;
    }

    public static string? GetCurrentData(this EntityEntry entry)
    {
        string? data = string.Empty;
        if (entry.Entity != null)
        {
            data = JsonConvert.SerializeObject(entry.CurrentValues.ToObject(), new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                MaxDepth = 7,
            });
        }
        return data;
    }
}
