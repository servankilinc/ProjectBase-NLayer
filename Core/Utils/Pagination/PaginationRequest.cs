namespace Core.Utils.Pagination;

public class PaginationRequest
{
    public int Page { get; set; } = 0;
    public int PageSize { get; set; } = 10;

    public PaginationRequest()
    {
    }

    public PaginationRequest(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
}
