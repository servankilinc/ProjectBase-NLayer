using Core.Model;
using Core.Utils.Datatable;
using Core.Utils.DynamicQuery;
using Core.Utils.Pagination;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Business.ServiceBase;

public interface IServiceBase<TEntity> where TEntity : class, IEntity
{
    #region Insert
    TEntity Add(TEntity entity);
    TDtoResponse Add<TDtoResponse>(TEntity entity) where TDtoResponse : IDto;
    TEntity Add<TDtoRequest>(TDtoRequest insertModel) where TDtoRequest : IDto;
    TDtoResponse Add<TDtoRequest, TDtoResponse>(TDtoRequest insertModel) where TDtoRequest : IDto where TDtoResponse : IDto;
    #endregion

    #region AddList
    List<TEntity> AddList(IEnumerable<TEntity> entityList);
    List<TDtoResponse> AddList<TDtoResponse>(IEnumerable<TEntity> entityList) where TDtoResponse : IDto;
    List<TEntity> AddList<TDtoRequest>(IEnumerable<TDtoRequest> insertModelList) where TDtoRequest : IDto;
    List<TDtoResponse> AddList<TDtoRequest, TDtoResponse>(IEnumerable<TDtoRequest> insertModelList) where TDtoRequest : IDto where TDtoResponse : IDto;
    #endregion

    #region Update
    TEntity Update(TEntity entity);
    TDtoResponse Update<TDtoResponse>(TEntity entity) where TDtoResponse : IDto;
    TEntity Update<TDtoRequest>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where) where TDtoRequest : IDto;
    TDtoResponse Update<TDtoRequest, TDtoResponse>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where) where TDtoRequest : IDto where TDtoResponse : IDto;
    #endregion

    #region UpdateList
    List<TEntity> UpdateList(IEnumerable<TEntity> entityList);
    List<TDtoResponse> UpdateList<TDtoResponse>(IEnumerable<TEntity> entityList) where TDtoResponse : IDto;
    #endregion

    #region Delete
    void Delete(TEntity entity);
    void Delete(IEnumerable<TEntity> entities);
    void Delete(Expression<Func<TEntity, bool>> where);
    #endregion

    #region IsExist & Count
    bool IsExist(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false);
    int Count(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false);
    #endregion

    #region Get
    TEntity? Get(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true
    );

    TDtoResponse? Get<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false
    ) where TDtoResponse : IDto;

    TDtoResponse? Get<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false
    ) where TDtoResponse : IDto;
    #endregion

    #region GetList
    ICollection<TEntity>? GetList(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true);

    ICollection<TDtoResponse>? GetList<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false) where TDtoResponse : IDto;

    ICollection<TDtoResponse>? GetList<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false) where TDtoResponse : IDto;
    #endregion

    #region Datatable Server-Side
    DatatableResponseServerSide<TEntity> DatatableServerSide(
        DatatableRequest datatableRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false);

    DatatableResponseServerSide<TDtoResponse> DatatableServerSide<TDtoResponse>(
       DatatableRequest datatableRequest,
       Expression<Func<TEntity, TDtoResponse>> select,
       Filter? filter = null,
       IEnumerable<Sort>? sorts = null,
       Expression<Func<TEntity, bool>>? where = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
       bool ignoreFilters = false) where TDtoResponse : IDto;

    DatatableResponseServerSide<TDtoResponse> DatatableServerSide<TDtoResponse>(
        DatatableRequest datatableRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto;
    #endregion

    #region Datatable Client-Side
    DatatableResponseClientSide<TEntity> DatatableClientSide(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false);

    DatatableResponseClientSide<TDtoResponse> DatatableClientSide<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto;

    DatatableResponseClientSide<TDtoResponse> DatatableClientSide<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto;
    #endregion

    #region Pagination
    PaginationResponse<TEntity> Pagination(
        PaginationRequest paginationRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false);
    PaginationResponse<TDtoResponse> Pagination<TDtoResponse>(
        PaginationRequest paginationRequest,
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto;
    PaginationResponse<TDtoResponse> Pagination<TDtoResponse>(
        PaginationRequest paginationRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto;
    #endregion
}
