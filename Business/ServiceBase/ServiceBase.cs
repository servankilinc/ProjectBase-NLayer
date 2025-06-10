using AutoMapper;
using Core.Model;
using Core.Utils.Datatable;
using Core.Utils.DynamicQuery;
using Core.Utils.Pagination;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Business.ServiceBase;

public class ServiceBase<TEntity, TRepository> : IServiceBase<TEntity>, IServiceBaseAsync<TEntity>
    where TEntity : class, IEntity
    where TRepository : IRepository<TEntity>, IRepositoryAsync<TEntity>
{
    private readonly TRepository _repository;
    private readonly IMapper _mapper;
    public ServiceBase(TRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }


    // ############################# Sync Methods #############################
    #region Add
    public TEntity _Add(TEntity entity)
    {
        return _repository.AddAndSave(entity);
    }

    public TDtoResponse _Add<TDtoResponse>(TEntity entity) where TDtoResponse : IDto
    {
        TEntity insertedEntity = _repository.AddAndSave(entity);
        return _mapper.Map<TDtoResponse>(insertedEntity);
    }

    public TEntity _Add<TDtoRequest>(TDtoRequest insertModel) where TDtoRequest : IDto
    {
        TEntity mappedEntity = _mapper.Map<TEntity>(insertModel);
        return _repository.AddAndSave(mappedEntity);
    }

    public TDtoResponse _Add<TDtoRequest, TDtoResponse>(TDtoRequest insertModel) where TDtoRequest : IDto where TDtoResponse : IDto
    {
        TEntity mappedEntity = _mapper.Map<TEntity>(insertModel);
        TEntity insertedEntity = _repository.AddAndSave(mappedEntity);
        return _mapper.Map<TDtoResponse>(insertedEntity);
    }
    #endregion

    #region AddList
    public List<TEntity> _AddList(IEnumerable<TEntity> entityList)
    {
        return _repository.AddAndSave(entityList);
    }

    public List<TDtoResponse> _AddList<TDtoResponse>(IEnumerable<TEntity> entityList) where TDtoResponse : IDto
    {
        List<TEntity> insertedEntityList = _repository.AddAndSave(entityList);
        return _mapper.Map<List<TDtoResponse>>(insertedEntityList);
    }

    public List<TEntity> _AddList<TDtoRequest>(IEnumerable<TDtoRequest> insertModelList) where TDtoRequest : IDto
    {
        IEnumerable<TEntity> mappedEntityList = _mapper.Map<IEnumerable<TEntity>>(insertModelList);
        return _repository.AddAndSave(mappedEntityList);
    }

    public List<TDtoResponse> _AddList<TDtoRequest, TDtoResponse>(IEnumerable<TDtoRequest> insertModelList) where TDtoRequest : IDto where TDtoResponse : IDto
    {
        IEnumerable<TEntity> mappedEntityList = _mapper.Map<IEnumerable<TEntity>>(insertModelList);
        List<TEntity> insertedEntityList = _repository.AddAndSave(mappedEntityList);
        return _mapper.Map<List<TDtoResponse>>(insertedEntityList);
    }
    #endregion

    #region Update
    public TEntity _Update(TEntity entity)
    {
        return _repository.UpdateAndSave(entity);
    }

    public TDtoResponse _Update<TDtoResponse>(TEntity entity) where TDtoResponse : IDto
    {
        TEntity updatedEntity = _repository.UpdateAndSave(entity);
        return _mapper.Map<TDtoResponse>(updatedEntity);
    }

    public TEntity _Update<TDtoRequest>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where) where TDtoRequest : IDto
    {
        TEntity? entity = _repository.Get(where: where);
        if (entity == null) throw new Exception($"The entity({nameof(TEntity)}) was not found to update. Update model => {JsonConvert.SerializeObject(updateModel)}.");

        TEntity entityToUpdate = _mapper.Map(updateModel, entity);
        return _repository.UpdateAndSave(entityToUpdate);
    }

    public TDtoResponse _Update<TDtoRequest, TDtoResponse>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where) where TDtoRequest : IDto where TDtoResponse : IDto
    {
        TEntity? entity = _repository.Get(where: where);
        if (entity == null) throw new Exception($"The entity({nameof(TEntity)}) was not found to update. Update model => {JsonConvert.SerializeObject(updateModel)}.");

        TEntity entityToUpdate = _mapper.Map(updateModel, entity);
        TEntity updatedEntity = _repository.UpdateAndSave(entityToUpdate);
        return _mapper.Map<TDtoResponse>(updatedEntity);
    }
    #endregion

    #region UpdateList
    public List<TEntity> _UpdateList(IEnumerable<TEntity> entityList)
    {
        return _repository.UpdateAndSave(entityList);
    }

    public List<TDtoResponse> _UpdateList<TDtoResponse>(IEnumerable<TEntity> entityList) where TDtoResponse : IDto
    {
        List<TEntity> updatedList = _repository.UpdateAndSave(entityList);
        return _mapper.Map<List<TDtoResponse>>(updatedList);
    }
    #endregion

    #region Delete
    public void _Delete(TEntity entity)
    {
        _repository.DeleteAndSave(entity);
    }

    public void _Delete(IEnumerable<TEntity> entityList)
    {
        _repository.DeleteAndSave(entityList);
    }

    public void _Delete(Expression<Func<TEntity, bool>> where)
    {
        _repository.DeleteAndSave(where);
    }
    #endregion

    #region IsExist & Count
    public bool _IsExist(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false)
    {
        return _repository.IsExist(filter, where, ignoreFilters);
    }

    public int _Count(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false)
    {
        return _repository.Count(filter, where, ignoreFilters);
    }
    #endregion

    #region Get
    public TEntity? _Get(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true)
    {
        TEntity? entity = _repository.Get(
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking);

        return entity;
    }

    public TDtoResponse? _Get<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false) where TDtoResponse : IDto
    {
        TDtoResponse? responseModel = _repository.Get(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking);

        return responseModel;
    }

    public TDtoResponse? _Get<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false) where TDtoResponse : IDto
    {
        TDtoResponse? responseModel = _repository.Get<TDtoResponse>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking);

        return responseModel;
    }
    #endregion

    #region GetList
    public ICollection<TEntity>? _GetList(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true)
    {
        ICollection<TEntity>? entity = _repository.GetAll(
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking);

        return entity;
    }

    public ICollection<TDtoResponse>? _GetList<TDtoResponse>(
       Expression<Func<TEntity, TDtoResponse>> select,
       Filter? filter = null,
       IEnumerable<Sort>? sorts = null,
       Expression<Func<TEntity, bool>>? where = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
       bool ignoreFilters = false,
       bool tracking = false) where TDtoResponse : IDto
    {
        ICollection<TDtoResponse>? responseModel = _repository.GetAll(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking);

        return responseModel;
    }

    public ICollection<TDtoResponse>? _GetList<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false) where TDtoResponse : IDto
    {
        ICollection<TDtoResponse>? responseModel = _repository.GetAll<TDtoResponse>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking);

        return responseModel;
    }
    #endregion

    #region Datatable Server-Side
    public DatatableResponseServerSide<TEntity> _DatatableServerSide(
        DatatableRequest datatableRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        return _repository.DatatableServerSide(
            datatableRequest: datatableRequest,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters);
    }

    public DatatableResponseServerSide<TDtoResponse> _DatatableServerSide<TDtoResponse>(
       DatatableRequest datatableRequest,
       Expression<Func<TEntity, TDtoResponse>> select,
       Filter? filter = null,
       IEnumerable<Sort>? sorts = null,
       Expression<Func<TEntity, bool>>? where = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
       bool ignoreFilters = false) where TDtoResponse : IDto
    {
        return _repository.DatatableServerSide<TDtoResponse>(
            datatableRequest: datatableRequest,
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters);
    }

    public DatatableResponseServerSide<TDtoResponse> _DatatableServerSide<TDtoResponse>(
        DatatableRequest datatableRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto
    {
        return _repository.DatatableServerSide<TDtoResponse>(
            datatableRequest: datatableRequest,
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters);
    }
    #endregion

    #region Datatable Client-Side
    public DatatableResponseClientSide<TEntity> _DatatableClientSide(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        return _repository.DatatableClientSide(
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters);
    }

    public DatatableResponseClientSide<TDtoResponse> _DatatableClientSide<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto
    {
        return _repository.DatatableClientSide<TDtoResponse>(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters);
    }

    public DatatableResponseClientSide<TDtoResponse> _DatatableClientSide<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto
    {
        return _repository.DatatableClientSide<TDtoResponse>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters);
    }
    #endregion

    #region Pagination
    public PaginationResponse<TEntity> _Pagination(
        PaginationRequest paginationRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false)
    {
        return _repository.Pagination(
            paginationRequest: paginationRequest,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters);
    }

    public PaginationResponse<TDtoResponse> _Pagination<TDtoResponse>(
        PaginationRequest paginationRequest,
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto
    {
        return _repository.Pagination<TDtoResponse>(
            paginationRequest: paginationRequest,
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters);
    }

    public PaginationResponse<TDtoResponse> _Pagination<TDtoResponse>(
        PaginationRequest paginationRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false) where TDtoResponse : IDto
    {
        return _repository.Pagination<TDtoResponse>(
            paginationRequest: paginationRequest,
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters);
    }
    #endregion

    // ############################# Async Methods #############################
    #region Add
    public async Task<TEntity> _AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return await _repository.AddAndSaveAsync(entity, cancellationToken);
    }

    public async Task<TDtoResponse> _AddAsync<TDtoResponse>(TEntity entity, CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        TEntity insertedEntity = await _repository.AddAndSaveAsync(entity, cancellationToken);
        return _mapper.Map<TDtoResponse>(insertedEntity);
    }

    public async Task<TEntity> _AddAsync<TDtoRequest>(TDtoRequest insertModel, CancellationToken cancellationToken = default) where TDtoRequest : IDto
    {
        TEntity mappedEntity = _mapper.Map<TEntity>(insertModel);
        return await _repository.AddAndSaveAsync(mappedEntity, cancellationToken);
    }

    public async Task<TDtoResponse> _AddAsync<TDtoRequest, TDtoResponse>(TDtoRequest insertModel, CancellationToken cancellationToken = default) where TDtoRequest : IDto where TDtoResponse : IDto
    {
        TEntity mappedEntity = _mapper.Map<TEntity>(insertModel);
        TEntity insertedEntity = await _repository.AddAndSaveAsync(mappedEntity, cancellationToken);
        return _mapper.Map<TDtoResponse>(insertedEntity);
    }
    #endregion

    #region AddList
    public async Task<List<TEntity>> _AddListAsync(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default)
    {
        return await _repository.AddAndSaveAsync(entityList, cancellationToken);
    }

    public async Task<List<TDtoResponse>> _AddListAsync<TDtoResponse>(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        List<TEntity> insertedEntityList = await _repository.AddAndSaveAsync(entityList, cancellationToken);
        return _mapper.Map<List<TDtoResponse>>(insertedEntityList);
    }

    public async Task<List<TEntity>> _AddListAsync<TDtoRequest>(IEnumerable<TDtoRequest> insertModelList, CancellationToken cancellationToken = default) where TDtoRequest : IDto
    {
        IEnumerable<TEntity> mappedEntityList = _mapper.Map<IEnumerable<TEntity>>(insertModelList);
        return await _repository.AddAndSaveAsync(mappedEntityList, cancellationToken);
    }

    public async Task<List<TDtoResponse>> _AddListAsync<TDtoRequest, TDtoResponse>(IEnumerable<TDtoRequest> insertModelList, CancellationToken cancellationToken = default) where TDtoRequest : IDto where TDtoResponse : IDto
    {
        IEnumerable<TEntity> mappedEntityList = _mapper.Map<IEnumerable<TEntity>>(insertModelList);
        List<TEntity> insertedEntityList = await _repository.AddAndSaveAsync(mappedEntityList, cancellationToken);
        return _mapper.Map<List<TDtoResponse>>(insertedEntityList);
    }
    #endregion

    #region Update
    public async Task<TEntity> _UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return await _repository.UpdateAndSaveAsync(entity, cancellationToken);
    }

    public async Task<TDtoResponse> _UpdateAsync<TDtoResponse>(TEntity entity, CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        TEntity updatedEntity = await _repository.UpdateAndSaveAsync(entity, cancellationToken);
        return _mapper.Map<TDtoResponse>(updatedEntity);
    }

    public async Task<TEntity> _UpdateAsync<TDtoRequest>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default) where TDtoRequest : IDto
    {
        TEntity? entity = await _repository.GetAsync(where: where, cancellationToken: cancellationToken);
        if (entity == null) throw new Exception($"The entity({nameof(TEntity)}) was not found to update. Update model => {JsonConvert.SerializeObject(updateModel)}.");

        TEntity entityToUpdate = _mapper.Map(updateModel, entity);
        return await _repository.UpdateAndSaveAsync(entityToUpdate, cancellationToken);
    }

    public async Task<TDtoResponse> _UpdateAsync<TDtoRequest, TDtoResponse>(TDtoRequest updateModel, Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default) where TDtoRequest : IDto where TDtoResponse : IDto
    {
        TEntity? entity = await _repository.GetAsync(where: where, cancellationToken: cancellationToken);
        if (entity == null) throw new Exception($"The entity({nameof(TEntity)}) was not found to update. Update model => {JsonConvert.SerializeObject(updateModel)}.");

        TEntity entityToUpdate = _mapper.Map(updateModel, entity);
        TEntity updatedEntity = await _repository.UpdateAndSaveAsync(entityToUpdate, cancellationToken);
        return _mapper.Map<TDtoResponse>(updatedEntity);
    }
    #endregion

    #region UpdateList
    public async Task<List<TEntity>> _UpdateListAsync(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default)
    {
        return await _repository.UpdateAndSaveAsync(entityList, cancellationToken);
    }

    public async Task<List<TDtoResponse>> _UpdateListAsync<TDtoResponse>(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        List<TEntity> updatedList = await _repository.UpdateAndSaveAsync(entityList, cancellationToken);
        return _mapper.Map<List<TDtoResponse>>(updatedList);
    }
    #endregion

    #region Delete
    public async Task _DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAndSaveAsync(entity, cancellationToken);
    }

    public async Task _DeleteAsync(IEnumerable<TEntity> entityList, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAndSaveAsync(entityList, cancellationToken);
    }

    public async Task _DeleteAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAndSaveAsync(where, cancellationToken);
    }
    #endregion

    #region IsExist & Count
    public async Task<bool> _IsExistAsync(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false, CancellationToken cancellationToken = default)
    {
        return await _repository.IsExistAsync(filter, where, ignoreFilters, cancellationToken);
    }

    public async Task<int> _CountAsync(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool ignoreFilters = false, CancellationToken cancellationToken = default)
    {
        return await _repository.CountAsync(filter, where, ignoreFilters, cancellationToken);
    }
    #endregion

    #region Get
    public async Task<TEntity?> _GetAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        TEntity? entity = await _repository.GetAsync(
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking,
            cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<TDtoResponse?> _GetAsync<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        TDtoResponse? responseModel = await _repository.GetAsync(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking,
            cancellationToken: cancellationToken);

        return responseModel;
    }

    public async Task<TDtoResponse?> _GetAsync<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        TDtoResponse? responseModel = await _repository.GetAsync<TDtoResponse>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking,
            cancellationToken: cancellationToken);

        return responseModel;
    }
    #endregion

    #region GetList
    public async Task<ICollection<TEntity>?> _GetListAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        ICollection<TEntity>? entity = await _repository.GetAllAsync(
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking,
            cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<ICollection<TDtoResponse>?> _GetListAsync<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        ICollection<TDtoResponse>? responseModel = await _repository.GetAllAsync(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking,
            cancellationToken: cancellationToken);

        return responseModel;
    }

    public async Task<ICollection<TDtoResponse>?> _GetListAsync<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        bool tracking = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        ICollection<TDtoResponse>? responseModel = await _repository.GetAllAsync<TDtoResponse>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            tracking: tracking,
            cancellationToken: cancellationToken);

        return responseModel;
    }
    #endregion

    #region Datatable Server-Side
    public async Task<DatatableResponseServerSide<TEntity>> _DatatableServerSideAsync(
        DatatableRequest datatableRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        return await _repository.DatatableServerSideAsync(
            datatableRequest: datatableRequest,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken);
    }

    public async Task<DatatableResponseServerSide<TDtoResponse>> _DatatableServerSideAsync<TDtoResponse>(
        DatatableRequest datatableRequest,
        Expression<Func<TEntity, TDtoResponse>> select, Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        return await _repository.DatatableServerSideAsync<TDtoResponse>(
            datatableRequest: datatableRequest,
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken);
    }

    public async Task<DatatableResponseServerSide<TDtoResponse>> _DatatableServerSideAsync<TDtoResponse>(
        DatatableRequest datatableRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        return await _repository.DatatableServerSideAsync<TDtoResponse>(
            datatableRequest: datatableRequest,
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken);
    }
    #endregion

    #region Datatable Client-Side
    public async Task<DatatableResponseClientSide<TEntity>> _DatatableClientSideAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        return await _repository.DatatableClientSideAsync(
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken);
    }

    public async Task<DatatableResponseClientSide<TDtoResponse>> _DatatableClientSideAsync<TDtoResponse>(
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        return await _repository.DatatableClientSideAsync<TDtoResponse>(
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken);
    }

    public async Task<DatatableResponseClientSide<TDtoResponse>> _DatatableClientSideAsync<TDtoResponse>(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        return await _repository.DatatableClientSideAsync<TDtoResponse>(
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken);
    }
    #endregion

    #region Pagination
    public async Task<PaginationResponse<TEntity>> _PaginationAsync(
        PaginationRequest paginationRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default)
    {
        return await _repository.PaginationAsync(
            paginationRequest: paginationRequest,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken);
    }

    public async Task<PaginationResponse<TDtoResponse>> _PaginationAsync<TDtoResponse>(
        PaginationRequest paginationRequest,
        Expression<Func<TEntity, TDtoResponse>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        return await _repository.PaginationAsync<TDtoResponse>(
            paginationRequest: paginationRequest,
            select: select,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken);
    }

    public async Task<PaginationResponse<TDtoResponse>> _PaginationAsync<TDtoResponse>(
        PaginationRequest paginationRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool ignoreFilters = false,
        CancellationToken cancellationToken = default) where TDtoResponse : IDto
    {
        return await _repository.PaginationAsync<TDtoResponse>(
            paginationRequest: paginationRequest,
            configurationProvider: _mapper.ConfigurationProvider,
            filter: filter,
            sorts: sorts,
            where: where,
            orderBy: orderBy,
            include: include,
            ignoreFilters: ignoreFilters,
            cancellationToken: cancellationToken);
    }
    #endregion
}
