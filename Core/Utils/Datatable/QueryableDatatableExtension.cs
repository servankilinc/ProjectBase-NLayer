using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Core.Utils.Datatable;

public static class QueryableDatatableExtension
{
    #region Server-Side Extension Methods
    // ***************** Sync Version *****************
    public static DatatableResponseServerSide<TData> ToDatatableServerSide<TData>(this IQueryable<TData> query, DatatableRequest dataTableRequest)
    {
        if (dataTableRequest == null) throw new ArgumentNullException(nameof(dataTableRequest));

        // 1. Count of Total Records
        int recordsTotal = query.Count();

        // 2. Filter by search parameter
        string? searchPredicate = GenerateSearchPredicate<TData>(dataTableRequest);
        if (searchPredicate != null) query = query.Where(searchPredicate, dataTableRequest.Search!.Value!.ToLower());

        // 3. Count of Filtered Records
        int recordsFiltered = query.Count();

        // 4. Ordering 
        string? orderPredicate = GenerateOrderPredicate<TData>(dataTableRequest);
        if (orderPredicate != null) query = query.OrderBy(orderPredicate);

        // 5. Pagination
        query = query.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

        return new DatatableResponseServerSide<TData>
        {
            Data = query.ToList(),
            Draw = dataTableRequest.Draw,
            RecordsTotal = recordsTotal,
            RecordsFiltered = recordsFiltered,
        };
    }

    // ***************** Async Version *****************
    public static async Task<DatatableResponseServerSide<TData>> ToDatatableServerSideAsync<TData>(this IQueryable<TData> query, DatatableRequest dataTableRequest, CancellationToken cancellationToken = default)
    {
        if (dataTableRequest == null) throw new ArgumentNullException(nameof(dataTableRequest));

        // 1. Count of Total Records
        int recordsTotal = await query.CountAsync();

        // 2. Filter by search parameter
        string? searchPredicate = GenerateSearchPredicate<TData>(dataTableRequest);
        if (searchPredicate != null) query = query.Where(searchPredicate, dataTableRequest.Search!.Value!.ToLower());

        // 3. Count of Filtered Records
        int recordsFiltered = await query.CountAsync();

        // 4. Ordering 
        string? orderPredicate = GenerateOrderPredicate<TData>(dataTableRequest);
        if (orderPredicate != null) query = query.OrderBy(orderPredicate);

        // 5. Pagination
        query = query.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

        var data = await query.ToListAsync(cancellationToken);

        return new DatatableResponseServerSide<TData>
        {
            Data = data,
            Draw = dataTableRequest.Draw,
            RecordsTotal = recordsTotal,
            RecordsFiltered = recordsFiltered,
        };
    }
    #endregion


    #region Client-Side Extension Methods
    // ***************** Sync Version *****************
    public static DatatableResponseClientSide<TData> ToDatatableClientSide<TData>(this IQueryable<TData> query)
    {
        return new DatatableResponseClientSide<TData>()
        {
            Data = query.ToList(),
        };
    }

    // ***************** Async Version *****************
    public static async Task<DatatableResponseClientSide<TData>> ToDatatableClientSideAsync<TData>(this IQueryable<TData> query, CancellationToken cancellationToken = default)
    {
        var data = await query.ToListAsync(cancellationToken);
        return new DatatableResponseClientSide<TData>()
        {
            Data = data,
        };
    }
    #endregion


    // ################# Helper Methods #################
    private static string? GenerateSearchPredicate<TData>(DatatableRequest dataTableRequest)
    {
        if (dataTableRequest.Search == null || string.IsNullOrEmpty(dataTableRequest.Search.Value) || dataTableRequest.Columns == null) return null;

        var props = typeof(TData).GetProperties().Select(p => p.Name).ToDictionary(p => p.ToLower(), p => p);

        IEnumerable<Column>? searchableColumns = dataTableRequest.Columns!.Where(c => c.Searchable && !string.IsNullOrEmpty(c.Data));

        foreach (var column in searchableColumns) // c.Data is column name
        {
            var key = column.Data!.ToLower();
            if (props.TryGetValue(key, out var actualPropName))
            {
                column.Data = actualPropName;
            }
        }
        var filters = searchableColumns.Select(c => $"{c.Data}.Contains(@0)");

        var searchPredicate = string.Join(" OR ", filters);
        return searchPredicate;
    }

    private static string? GenerateOrderPredicate<TData>(DatatableRequest dataTableRequest)
    {
        if (dataTableRequest.Order == null || dataTableRequest.Columns == null) return null;
        
        var props = typeof(TData).GetProperties().Select(p => p.Name).ToDictionary(p => p.ToLower(), p => p);

        List<string> orderList = new List<string>();
        foreach (var orderItem in dataTableRequest.Order)
        {
            var column = dataTableRequest.Columns[orderItem.Column];
            if (column == null || !column.Orderable || string.IsNullOrEmpty(column.Data)) continue;

            var key = column.Data.ToLower();
            if (props.TryGetValue(key, out var actualPropName))
            {
                orderList.Add($"{actualPropName} {orderItem.Dir}");
            }
        }
        string orderPredicate = string.Join(",", orderList);

        if (orderList.Any()) return orderPredicate;
        return null;
    }
}