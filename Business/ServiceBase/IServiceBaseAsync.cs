using Core.Model;
using Core.Utils.Datatable;
using Core.Utils.DynamicQuery;
using Core.Utils.Pagination;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Business.ServiceBase;

public interface IServiceBaseAsync<TEntity> where TEntity : class, IEntity
{
    #region Insert
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TDtoResponse> AddAsync<TDtoResponse>(TEntity entity, CancellationToken cancellationToken = default) where TDtoResponse : IDto;
    Task<TEntity> AddAsync<TDtoRequest>(TDtoRequest insertModel, CancellationToken cancellationToken = default) where TDtoRequest : IDto;
    Task<TDtoResponse> AddAsync<TDtoRequest, TDtoResponse>(TDtoRequest insertModel, CancellationToken cancellationToken = default) where TDtoRequest : IDto where TDtoResponse : IDto;
    #endregion

    #region AddList
    Task<List<TEntity>> AddListAsync(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default);
    Task<List<TDtoResponse>> AddListAsync<TDtoResponse>(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default) where TDtoResponse : IDto;
    Task<List<TEntity>> AddListAsync<TDtoRequest>(IEnumerable<TDtoRequest> insertModelList, CancellationToken cancellationToken = default) where TDtoRequest : IDto;
    Task<List<TDtoResponse>> AddListAsync<TDtoRequest, TDtoResponse>(IEnumerable<TDtoRequest> insertModelList, CancellationToken cancellationToken = default) where TDtoRequest : IDto where TDtoResponse : IDto;
    #endregion

    #region Update
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TDtoResponse> UpdateAsync<TDtoResponse>(TEntity entity, CancellationToken cancellationToken = default) where TDtoResponse : IDto;
    Task<TEntity> UpdateAsync<TDtoRequest>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default) where TDtoRequest : IDto;
    Task<TDtoResponse> UpdateAsync<TDtoRequest, TDtoResponse>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default) where TDtoRequest : IDto where TDtoResponse : IDto;
    #endregion

    #region UpdateList
    Task<List<TEntity>> UpdateListAsync(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default);
    Task<List<TDtoResponse>> UpdateListAsync<TDtoResponse>(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default) where TDtoResponse : IDto;
    #endregion

    #region Delete
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default);
    Task DeleteAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default);
    #endregion

    #region IsExist & Count
    Task<bool> IsExistAsync(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false, CancellationToken cancellationToken = default);
    #endregion

    #region Get
    Task<TEntity?> GetAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true,
        CancellationToken cancellationToken = default);

    Task<TDtoResponse?> GetAsync<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto;

    Task<TDtoResponse?> GetAsync<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto;
    #endregion

    #region GetList
    Task<ICollection<TEntity>?> GetListAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true,
        CancellationToken cancellationToken = default);

    Task<ICollection<TDtoResponse>?> GetListAsync<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto;

    Task<ICollection<TDtoResponse>?> GetListAsync<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto;
    #endregion

    #region Datatable Server-Side
    Task<DatatableResponseServerSide<TEntity>> DatatableServerSideAsync(
        DatatableRequest datatableRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default);

    Task<DatatableResponseServerSide<TDtoResponse>> DatatableServerSideAsync<TDtoResponse>(
       DatatableRequest datatableRequest,
       Expression<Func<TEntity, TDtoResponse>> select,
       Filter? filter = null,
       IEnumerable<Sort>? sorts = null,
       Expression<Func<TEntity, bool>>? where = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
       bool ignoreFilters = false,
       CancellationToken cancellationToken = default) where TDtoResponse : IDto;

    Task<DatatableResponseServerSide<TDtoResponse>> DatatableServerSideAsync<TDtoResponse>(
        DatatableRequest datatableRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto;
    #endregion

    #region Datatable Client-Side
    Task<DatatableResponseClientSide<TEntity>> DatatableClientSideAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default);

    Task<DatatableResponseClientSide<TDtoResponse>> DatatableClientSideAsync<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto;

    Task<DatatableResponseClientSide<TDtoResponse>> DatatableClientSideAsync<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto;
    #endregion

    #region Pagination
    Task<PaginationResponse<TEntity>> PaginationAsync(
        PaginationRequest paginationRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default);

    Task<PaginationResponse<TDtoResponse>> PaginationAsync<TDtoResponse>(
        PaginationRequest paginationRequest,
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto;

    Task<PaginationResponse<TDtoResponse>> PaginationAsync<TDtoResponse>(
        PaginationRequest paginationRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto;
    #endregion
}
