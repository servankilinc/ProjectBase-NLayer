using Microsoft.EntityFrameworkCore;

namespace Core.Utils.Pagination;

public static class QueryablePaginationExtension
{
    public static PaginationResponse<TData> ToPaginate<TData>(this IQueryable<TData> queryable, PaginationRequest request)
    {
        int count = queryable.Count();

        if (request.Page == default || request.Page < 0) request.Page = 0;
        if (request.PageSize == default || request.PageSize <= 0) request.PageSize = count;

        List<TData> items = queryable.Skip(request.Page * request.PageSize).Take(request.PageSize).ToList();
        PaginationResponse<TData> list = new()
        {
            Page = request.Page,
            PageSize = request.PageSize,
            DataCount = count,
            Data = items,
            PageCount = (count <= 0 || request.PageSize <= 0) ? 0 : (int)Math.Ceiling(count / (double)request.PageSize)
        };
        return list;
    }
 
    public static async Task<PaginationResponse<TData>> ToPaginateAsync<TData>(this IQueryable<TData> queryable, PaginationRequest request, CancellationToken cancellationToken = default)
    {
        int count = await queryable.CountAsync(cancellationToken);

        if (request.Page == default || request.Page < 0) request.Page = 0;

        if (request.PageSize == default || request.PageSize <= 0) request.PageSize = count;

        List<TData> items = await queryable.Skip(request.Page * request.PageSize).Take(request.PageSize).ToListAsync(cancellationToken);

        return new PaginationResponse<TData>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            DataCount = count,
            Data = items,
            PageCount = (count <= 0 || request.PageSize <= 0) ? 0 : (int)Math.Ceiling(count / (double)request.PageSize)
        };
    }
}
