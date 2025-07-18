using Business.Abstract;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Blog_;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;
    public BlogController(IBlogService blogService) => _blogService = blogService;

    #region GetEntity
    [HttpGet("Get")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _blogService.GetAsync(id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll(DynamicRequest? request)
    {
        var result = await _blogService.GetAllAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetList")]
    public async Task<IActionResult> GetList(DynamicPaginationRequest request)
    {
        var result = await _blogService.GetListAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region GetBasic
    [HttpGet("GetByBasic")]
    public async Task<IActionResult> GetByBasic(Guid Id)
    {
        var result = await _blogService.GetByBasicAsync(Id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetAllByBasic")]
    public async Task<IActionResult> GetAllByBasic(DynamicRequest? request)
    {
        var result = await _blogService.GetAllByBasicAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetListByBasic")]
    public async Task<IActionResult> GetListByBasic(DynamicPaginationRequest request)
    {
        var result = await _blogService.GetListByBasicAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region GetDetail
    [HttpGet("GetByDetail")]
    public async Task<IActionResult> GetByDetail(Guid Id)
    {
        var result = await _blogService.GetByDetailAsync(Id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetAllByDetail")]
    public async Task<IActionResult> GetAllByDetail(DynamicRequest? request)
    {
        var result = await _blogService.GetAllByDetailAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetListByDetail")]
    public async Task<IActionResult> GetListByDetail(DynamicPaginationRequest request)
    {
        var result = await _blogService.GetListByDetailAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region Create
    [HttpPost("Create")]
    public async Task<IActionResult> Create(BlogCreateDto request)
    {
        var result = await _blogService.CreateAsync(request);

        return Ok(result);
    }
    #endregion

    #region Update
    [HttpPatch("Update")]
    public async Task<IActionResult> Update(BlogUpdateDto request)
    {
        var result = await _blogService.UpdateAsync(request);

        return Ok(result);
    }
    #endregion

    #region Delete
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(Guid Id)
    {
        await _blogService.DeleteAsync(Id);

        return Ok();
    }
    #endregion

    #region Datatable Methods
    [HttpPost("DatatableClientSide")]
    public async Task<IActionResult> DatatableClientSide(DynamicRequest request)
    {
        var result = await _blogService.DatatableClientSideAsync(request);

        return Ok(result);
    }
    
    [HttpPost("DatatableClientSideReport")]
    public async Task<IActionResult> DatatableClientSideReport(DynamicRequest request)
    {
        var result = await _blogService.DatatableClientSideByReportAsync(request);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost("DatatableServerSide")]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableServerSideRequest request)
    {
        var result = await _blogService.DatatableServerSideAsync(request);

        return Ok(result);
    } 

    [HttpPost("DatatableServerSideReport")]
    public async Task<IActionResult> DatatableServerSideReport(DynamicDatatableServerSideRequest request)
    {
        var result = await _blogService.DatatableServerSideByReportAsync(request);
        if (result == null) return NotFound();
        return Ok(result);
    }
    #endregion
}
