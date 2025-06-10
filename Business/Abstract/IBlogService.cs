using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Model.Dtos.Blog_;
using Model.Entities;

namespace Business.Abstract;

public interface IBlogService : IServiceBase<Blog>, IServiceBaseAsync<Blog>
{
    #region GetBasic
    Task<BlogBasicResponseDto?> GetAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<BlogBasicResponseDto>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<BlogBasicResponseDto>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region GetDetail
    Task<BlogDetailResponseDto?> GetByDetailAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<BlogDetailResponseDto>?> GetAllByDetailAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<BlogDetailResponseDto>> GetListByDetailAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region Create
    Task<Blog> CreateAsync(BlogCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<Blog> UpdateAsync(BlogUpdateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    #endregion

    #region Datatable Methods
    Task<DatatableResponseClientSide<Blog>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<Blog>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default);
    #endregion
}
