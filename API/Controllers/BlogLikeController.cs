using Business.Abstract;
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

    #region GetBasic
    [HttpGet("Get")]
    public async Task<IActionResult> Get(Guid BlogId, Guid UserId)
    {
        var result = await _blogLikeService.GetAsync(BlogId, UserId);

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
