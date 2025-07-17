using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Model;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Dtos.Blog_;
using Model.Dtos.Category_;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface ICategoryService : IServiceBase<Category>, IServiceBaseAsync<Category>
{
    #region Get Entity
    Task<Category?> GetAsync(Expression<Func<Category, bool>> where, CancellationToken cancellationToken = default);
    Task<ICollection<Category>?> GetListAsync(Expression<Func<Category, bool>>? where = default, CancellationToken cancellationToken = default);
    #endregion

    #region Get Generic
    Task<TResponse?> GetAsync<TResponse>(Guid Id, CancellationToken cancellationToken = default) where TResponse : IDto;
    Task<ICollection<TResponse>?> GetListAsync<TResponse>(Expression<Func<Category, bool>>? where = default, CancellationToken cancellationToken = default) where TResponse : IDto;
    #endregion

    #region SelectList
    Task<SelectList> GetSelectListAsync(Expression<Func<Category, bool>>? where = default, CancellationToken cancellationToken = default);
    #endregion

    #region GetBasic
    Task<CategoryResponseDto?> GetByBasicAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<CategoryResponseDto>?> GetAllByBasicAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<CategoryResponseDto>> GetListByBasicAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region GetDetail
    Task<CategoryBlogsResponseDto?> GetByDetailAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<CategoryBlogsResponseDto>?> GetAllByDetailAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<CategoryBlogsResponseDto>> GetListByDetailAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region Create
    Task<CategoryResponseDto> CreateAsync(CategoryCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<CategoryResponseDto> UpdateAsync(CategoryUpdateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    #endregion

    #region Datatable Methods
    Task<DatatableResponseClientSide<Category>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseClientSide<CategoryReportDto>> DatatableClientSideByReportAsync(DynamicRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<Category>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<CategoryReportDto>> DatatableServerSideByReportAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default);
    #endregion
}
