using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Core.Utils.Datatable;

public static class QueryableDatatableExtension
{
    #region Server-Side Extension Methods
    
    // ***************** Sync Version *****************
    public static DatatableResponseServerSide<TData> ToDatatableServerSide<TData>(this IQueryable<TData> query, DatatableRequest dataTableRequest)
    {
        if (dataTableRequest == null) throw new ArgumentNullException(nameof(DatatableRequest));
        var _response = new DatatableResponseServerSide<TData>();

        // 1. Count of Total Records
        int recordsTotal = query.Count();

        // 2. Filter by search parameter
        if (dataTableRequest.Search != null && !string.IsNullOrEmpty(dataTableRequest.Search.Value) && dataTableRequest.Columns != null)
        {
            var props = typeof(TData).GetProperties().Select(p => p.Name).ToDictionary(p => p.ToLower(), p => p);

            foreach (var column in dataTableRequest.Columns.Where(c => c.Searchable && !string.IsNullOrWhiteSpace(c.Data)))
            {
                var key = column.Data!.ToLower();
                if (props.TryGetValue(key, out var actualPropName))
                {
                    column.Data = actualPropName;
                }
            }
            var filters = dataTableRequest.Columns
                .Where(c => c.Searchable && c.Data != null && props.Any(p => p.Value == c.Data))
                .Select(c => $"{c.Data}.Contains(@0.ToLower())");

            var searchPredicate = string.Join(" OR ", filters);
            query = query.Where(searchPredicate, dataTableRequest.Search.Value.ToLower());
        }

        // 3. Count of Filtered Records
        int recordsFiltered = query.Count();

        // 4. Ordering 
        if (dataTableRequest.Order != null && dataTableRequest.Columns != null)
        {
            List<string> orderList = new List<string>();
            foreach (var orderItem in dataTableRequest.Order)
            {
                var column = dataTableRequest.Columns[orderItem.Column];
                if (column != null && column.Orderable && !string.IsNullOrEmpty(column.Data))
                    orderList.Add($"{column.Data} {orderItem.Dir}");
            }
            if (orderList.Count > 0)
            {
                string concatedOrders = string.Join(",", orderList);
                query = query.OrderBy(concatedOrders);
            }
        }

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

        var _response = new DatatableResponseServerSide<TData>();

        // 1. Count of Total Records
        int recordsTotal = await query.CountAsync(cancellationToken);

        // 2. Filter by search parameter
        if (dataTableRequest.Search != null && !string.IsNullOrEmpty(dataTableRequest.Search.Value) && dataTableRequest.Columns != null)
        {
            var props = typeof(TData).GetProperties().Select(p => p.Name).ToDictionary(p => p.ToLower(), p => p);

            foreach (var column in dataTableRequest.Columns.Where(c => c.Searchable && !string.IsNullOrWhiteSpace(c.Data)))
            {
                var key = column.Data!.ToLower();
                if (props.TryGetValue(key, out var actualPropName))
                {
                    column.Data = actualPropName;
                }
            }

            var filters = dataTableRequest.Columns
                .Where(c => c.Searchable && c.Data != null && props.Any(p => p.Value == c.Data))
                .Select(c => $"{c.Data}.ToLower().Contains(@0)");

            var searchPredicate = string.Join(" OR ", filters);
            query = query.Where(searchPredicate, dataTableRequest.Search.Value.ToLower());
        }

        // 3. Count of Filtered Records
        int recordsFiltered = await query.CountAsync(cancellationToken);

        // 4. Ordering
        if (dataTableRequest.Order != null && dataTableRequest.Columns != null)
        {
            List<string> orderList = new List<string>();
            foreach (var orderItem in dataTableRequest.Order)
            {
                var column = dataTableRequest.Columns[orderItem.Column];
                if (column != null && column.Orderable && !string.IsNullOrEmpty(column.Data))
                    orderList.Add($"{column.Data} {orderItem.Dir}");
            }

            if (orderList.Count > 0)
            {
                string concatedOrders = string.Join(",", orderList);
                query = query.OrderBy(concatedOrders);
            }
        }

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
}