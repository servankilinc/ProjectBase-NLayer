namespace Core.Utils.Pagination;

public class PaginationRequest
{
    public int Page { get; set; } = 1;
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
