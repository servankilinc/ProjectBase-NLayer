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
    TEntity _Add(TEntity entity);
    TDtoResponse _Add<TDtoResponse>(TEntity entity) where TDtoResponse : IDto;
    TEntity _Add<TDtoRequest>(TDtoRequest insertModel) where TDtoRequest : IDto;
    TDtoResponse _Add<TDtoRequest, TDtoResponse>(TDtoRequest insertModel) where TDtoRequest : IDto where TDtoResponse : IDto;
    #endregion

    #region AddList
    List<TEntity> _AddList(IEnumerable<TEntity> entityList);
    List<TDtoResponse> _AddList<TDtoResponse>(IEnumerable<TEntity> entityList) where TDtoResponse : IDto;
    List<TEntity> _AddList<TDtoRequest>(IEnumerable<TDtoRequest> insertModelList) where TDtoRequest : IDto;
    List<TDtoResponse> _AddList<TDtoRequest, TDtoResponse>(IEnumerable<TDtoRequest> insertModelList) where TDtoRequest : IDto where TDtoResponse : IDto;
    #endregion

    #region Update
    TEntity _Update(TEntity entity, Expression<Func<TEntity, bool>> where);
    TDtoResponse _Update<TDtoResponse>(TEntity entity, Expression<Func<TEntity, bool>> where) where TDtoResponse : IDto;
    TEntity _Update<TDtoRequest>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where) where TDtoRequest : IDto;
    TDtoResponse _Update<TDtoRequest, TDtoResponse>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where) where TDtoRequest : IDto where TDtoResponse : IDto;
    #endregion

    #region UpdateList
    List<TEntity> _UpdateList(IEnumerable<TEntity> entityList);
    List<TDtoResponse> _UpdateList<TDtoResponse>(IEnumerable<TEntity> entityList) where TDtoResponse : IDto;
    #endregion

    #region Delete
    void _Delete(TEntity entity);
    void _Delete(IEnumerable<TEntity> entities);
    void _Delete(Expression<Func<TEntity, bool>> where);
    #endregion

    #region IsExist & Count
    bool _IsExist(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false);
    int _Count(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false);
    #endregion

    #region Get
    TEntity? _Get(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true
    );

    TDtoResponse? _Get<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false
    ) where TDtoResponse : IDto;

    TDtoResponse? _Get<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false
    ) where TDtoResponse : IDto;
    #endregion

    #region GetList
    ICollection<TEntity>? _GetList(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true);

    ICollection<TDtoResponse>? _GetList<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false) where TDtoResponse : IDto;

    ICollection<TDtoResponse>? _GetList<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false) where TDtoResponse : IDto;
    #endregion

    #region Datatable Server-Side
    DatatableResponseServerSide<TEntity> _DatatableServerSide(
        DatatableRequest datatableRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false);

    DatatableResponseServerSide<TDtoResponse> _DatatableServerSide<TDtoResponse>(
       DatatableRequest datatableRequest,
       Expression<Func<TEntity, TDtoResponse>> select,
       Filter? filter = null,
       IEnumerable<Sort>? sorts = null,
       Expression<Func<TEntity, bool>>? where = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
       bool ignoreFilters = false) where TDtoResponse : IDto;

    DatatableResponseServerSide<TDtoResponse> _DatatableServerSide<TDtoResponse>(
        DatatableRequest datatableRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto;
    #endregion

    #region Datatable Client-Side
    DatatableResponseClientSide<TEntity> _DatatableClientSide(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false);

    DatatableResponseClientSide<TDtoResponse> _DatatableClientSide<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto;

    DatatableResponseClientSide<TDtoResponse> _DatatableClientSide<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto;
    #endregion

    #region Pagination
    PaginationResponse<TEntity> _Pagination(
        PaginationRequest paginationRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false);
    PaginationResponse<TDtoResponse> _Pagination<TDtoResponse>(
        PaginationRequest paginationRequest,
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto;
    PaginationResponse<TDtoResponse> _Pagination<TDtoResponse>(
        PaginationRequest paginationRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto;
    #endregion
}
