using Business.Abstract;
using Business.Concrete;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.BlogLike_;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BlogLikeController : ControllerBase
{
    private readonly IBlogLikeService _blogLikeService;
    public BlogLikeController(IBlogLikeService blogLikeService) => _blogLikeService = blogLikeService;


    #region GetEntity
    [HttpGet("Get")]
    public async Task<IActionResult> Get(Guid blogId, Guid userId)
    {
        var result = await _blogLikeService.GetAsync(BlogId: blogId, UserId: userId);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll(DynamicRequest? request)
    {
        var result = await _blogLikeService.GetAllAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetList")]
    public async Task<IActionResult> GetList(DynamicPaginationRequest request)
    {
        var result = await _blogLikeService.GetListAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion


    #region GetBasic
    [HttpGet("GetByBasic")]
    public async Task<IActionResult> GetByBasic(Guid BlogId, Guid UserId)
    {
        var result = await _blogLikeService.GetByBasicAsync(BlogId, UserId);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetAllByBasic")]
    public async Task<IActionResult> GetAllByBasic(DynamicRequest? request)
    {
        var result = await _blogLikeService.GetAllByBasicAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetListByBasic")]
    public async Task<IActionResult> GetListByBasic(DynamicPaginationRequest request)
    {
        var result = await _blogLikeService.GetListByBasicAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region GetDetail
    [HttpGet("GetByDetail")]
    public async Task<IActionResult> GetByDetail(Guid BlogId, Guid UserId)
    {
        var result = await _blogLikeService.GetByDetailAsync(BlogId, UserId);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetAllByDetail")]
    public async Task<IActionResult> GetAllByDetail(DynamicRequest? request)
    {
        var result = await _blogLikeService.GetAllByDetailAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetListByDetail")]
    public async Task<IActionResult> GetListByDetail(DynamicPaginationRequest request)
    {
        var result = await _blogLikeService.GetListByDetailAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region Create
    [HttpPost("Create")]
    public async Task<IActionResult> Create(BlogLikeCreateDto request)
    {
        var result = await _blogLikeService.CreateAsync(request);

        return Ok(result);
    }
    #endregion

    #region Delete
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(BlogLikeDeleteDto request)
    {
        await _blogLikeService.DeleteAsync(request);

        return Ok();
    }
    #endregion

    #region Datatable Methods
    [HttpPost("DatatableClientSide")]
    public async Task<IActionResult> DatatableClientSide(DynamicRequest request)
    {
        var result = await _blogLikeService.DatatableClientSideAsync(request);

        return Ok(result);
    }

    [HttpPost("DatatableServerSide")]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableServerSideRequest request)
    {
        var result = await _blogLikeService.DatatableServerSideAsync(request);

        return Ok(result);
    }
    #endregion
}
