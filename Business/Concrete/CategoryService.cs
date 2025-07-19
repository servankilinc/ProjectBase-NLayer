using AutoMapper;
using Business.Abstract;
using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Model;
using Core.Utils.CrossCuttingConcerns;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.Dtos.Category_;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

[ExceptionHandler]
public class CategoryService : ServiceBase<Category, ICategoryRepository>, ICategoryService
{
    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper) : base(categoryRepository, mapper)
    {
    }

    #region Get Entity
    public async Task<Category?> GetAsync(Expression<Func<Category, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await _GetAsync(
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<Category>?> GetListAsync(Expression<Func<Category, bool>>? where = null, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync(
             where: where,
             tracking: false,
             cancellationToken: cancellationToken
         );

        return result;
    }
    #endregion

    #region Get Generic
    public async Task<TResponse?> GetAsync<TResponse>(Expression<Func<Category, bool>> where, CancellationToken cancellationToken = default) where TResponse : IDto
    {
        var result = await _GetAsync<TResponse>(
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<TResponse>?> GetListAsync<TResponse>(Expression<Func<Category, bool>>? where = null, CancellationToken cancellationToken = default) where TResponse : IDto
    {
        var result = await _GetListAsync<TResponse>(
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region SelectList
    public async Task<SelectList> GetSelectListAsync(Expression<Func<Category, bool>>? where = default, CancellationToken cancellationToken = default)
    {
        var result = new SelectList(await _GetListAsync(
            select: s => new
            {
                s.Id,
                s.Name
            },
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        ), "Id", "Name");

        return result;
    }
    #endregion

    #region Get
    public async Task<Category?> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == default) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync(
            where: f => f.Id == Id,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<Category>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<Category>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region GetBasic
    public async Task<CategoryResponseDto?> GetByBasicAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == default) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync<CategoryResponseDto>(
            where: f => f.Id == Id,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<CategoryResponseDto>?> GetAllByBasicAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<CategoryResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<CategoryResponseDto>> GetListByBasicAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync<CategoryResponseDto>(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region GetDetail
    public async Task<CategoryBlogsResponseDto?> GetByDetailAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == Guid.Empty) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync<CategoryBlogsResponseDto>(
            where: f => f.Id == Id,
            include: i => i
                .Include(x => x.Blogs),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<CategoryBlogsResponseDto>?> GetAllByDetailAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<CategoryBlogsResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Blogs),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<CategoryBlogsResponseDto>> GetListByDetailAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync<CategoryBlogsResponseDto>(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            include: i => i
                .Include(x => x.Blogs),
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region Create
    [Validation(typeof(CategoryCreateDto))]
    public async Task<CategoryResponseDto> CreateAsync(CategoryCreateDto request, CancellationToken cancellationToken = default)
    {
        var result = await _AddAsync<CategoryCreateDto, CategoryResponseDto>(request, cancellationToken);

        return result;
    }
    #endregion

    #region Update
    [Validation(typeof(CategoryUpdateDto))]
    public async Task<CategoryResponseDto> UpdateAsync(CategoryUpdateDto request, CancellationToken cancellationToken = default)
    {
        var result = await _UpdateAsync<CategoryUpdateDto, CategoryResponseDto>(updateModel: request, where: f => f.Id == request.Id, cancellationToken);

        return result;
    }
    #endregion

    #region Delete
    public async Task DeleteAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == Guid.Empty) throw new ArgumentNullException(nameof(Id));

        await _DeleteAsync(where: f => f.Id == Id, cancellationToken);
    }
    #endregion

    #region Datatable Methods
    public async Task<DatatableResponseClientSide<Category>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableClientSideAsync(
            filter: request.Filter,
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<DatatableResponseClientSide<CategoryReportDto>> DatatableClientSideByReportAsync(DynamicRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableClientSideAsync<CategoryReportDto>(
            filter: request.Filter,
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<DatatableResponseServerSide<Category>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableServerSideAsync(
            datatableRequest: request.GetDatatableRequest(),
            filter: request.Filter,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<DatatableResponseServerSide<CategoryReportDto>> DatatableServerSideByReportAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableServerSideAsync<CategoryReportDto>(
            datatableRequest: request.GetDatatableRequest(),
            filter: request.Filter,
            cancellationToken: cancellationToken
        );

        return result;

    }
    #endregion
}
