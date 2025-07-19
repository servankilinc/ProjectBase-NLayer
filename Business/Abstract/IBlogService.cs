using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Model;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Dtos.Blog_;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IBlogService : IServiceBase<Blog>, IServiceBaseAsync<Blog>
{
    #region Get Entity
    Task<Blog?> GetAsync(Expression<Func<Blog, bool>> where, CancellationToken cancellationToken = default);
    Task<ICollection<Blog>?> GetListAsync(Expression<Func<Blog, bool>>? where = default, CancellationToken cancellationToken = default);
    #endregion

    #region Get Generic
    Task<TResponse?> GetAsync<TResponse>(Expression<Func<Blog, bool>> where, CancellationToken cancellationToken = default) where TResponse : IDto;
    Task<ICollection<TResponse>?> GetListAsync<TResponse>(Expression<Func<Blog, bool>>? where = default, CancellationToken cancellationToken = default) where TResponse : IDto;
    #endregion

    #region SelectList
    Task<SelectList> GetSelectListAsync(Expression<Func<Blog, bool>>? where = default, CancellationToken cancellationToken = default);
    #endregion

    #region Get
    Task<Blog?> GetAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<Blog>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<Blog>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region GetBasic
    Task<BlogBasicResponseDto?> GetByBasicAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<BlogBasicResponseDto>?> GetAllByBasicAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<BlogBasicResponseDto>> GetListByBasicAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region GetDetail
    Task<BlogDetailResponseDto?> GetByDetailAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<BlogDetailResponseDto>?> GetAllByDetailAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<BlogDetailResponseDto>> GetListByDetailAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region Create
    Task<BlogBasicResponseDto> CreateAsync(BlogCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<BlogBasicResponseDto> UpdateAsync(BlogUpdateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    #endregion

    #region Datatable Methods
    Task<DatatableResponseClientSide<Blog>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseClientSide<BlogReportDto>> DatatableClientSideByReportAsync(DynamicRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<Blog>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<BlogReportDto>> DatatableServerSideByReportAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default);
    #endregion
}
