using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Model.Dtos.BlogComment_;
using Model.Entities;

namespace Business.Abstract;

public interface IBlogCommentService : IServiceBase<BlogComment>, IServiceBaseAsync<BlogComment>
{
    #region GetBasic
    Task<BlogCommentBasicResponseDto?> GetAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<BlogCommentBasicResponseDto>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<BlogCommentBasicResponseDto>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region GetDetail
    Task<BlogCommentDetailResponseDto?> GetByDetailAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<BlogCommentDetailResponseDto>?> GetAllByDetailAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<BlogCommentDetailResponseDto>> GetListByDetailAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region Create
    Task<BlogCommentBasicResponseDto> CreateAsync(BlogCommentCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<BlogCommentBasicResponseDto> UpdateAsync(BlogCommentUpdateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task DeleteAsync(Guid Id, CancellationToken cancellationToken = default); 
    #endregion

    #region Datatable Methods
    Task<DatatableResponseClientSide<BlogComment>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<BlogComment>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default); 
    #endregion
}
