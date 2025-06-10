using Core.Utils.Datatable;
using Core.Utils.DynamicQuery;
using Core.Utils.Pagination;

namespace Core.BaseRequestModels;

public class DynamicRequest
{
    public Filter? Filter { get; set; }
    public IEnumerable<Sort>? Sorts { get; set; }
}

public class DynamicPaginationRequest
{
    public PaginationRequest PaginationRequest { get; set; } = new PaginationRequest();
    public Filter? Filter { get; set; }
    public IEnumerable<Sort>? Sorts { get; set; }
}

public class DynamicDatatableServerSideRequest
{
    public DatatableRequest DatatableRequest { get; set; } = null!;
    public Filter? Filter { get; set; }
}