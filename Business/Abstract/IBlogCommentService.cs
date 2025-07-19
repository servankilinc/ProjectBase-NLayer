using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Model;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Dtos.BlogComment_;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IBlogCommentService : IServiceBase<BlogComment>, IServiceBaseAsync<BlogComment>
{
    #region Get Entity
    Task<BlogComment?> GetAsync(Expression<Func<BlogComment, bool>> where, CancellationToken cancellationToken = default);
    Task<ICollection<BlogComment>?> GetListAsync(Expression<Func<BlogComment, bool>>? where = default, CancellationToken cancellationToken = default);
    #endregion

    #region Get Generic
    Task<TResponse?> GetAsync<TResponse>(Expression<Func<BlogComment, bool>> where, CancellationToken cancellationToken = default) where TResponse : IDto;
    Task<ICollection<TResponse>?> GetListAsync<TResponse>(Expression<Func<BlogComment, bool>>? where = default, CancellationToken cancellationToken = default) where TResponse : IDto;
    #endregion

    #region SelectList
    Task<SelectList> GetSelectListAsync(Expression<Func<BlogComment, bool>>? where = default, CancellationToken cancellationToken = default);
    #endregion

    #region Get
    Task<BlogComment?> GetAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<BlogComment>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<BlogComment>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region GetBasic
    Task<BlogCommentBasicResponseDto?> GetByBasicAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<BlogCommentBasicResponseDto>?> GetAllByBasicAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<BlogCommentBasicResponseDto>> GetListByBasicAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
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
    Task<DatatableResponseClientSide<BlogCommentReportDto>> DatatableClientSideByReportAsync(DynamicRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<BlogComment>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<BlogCommentReportDto>> DatatableServerSideByReportAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default);
    #endregion
}
