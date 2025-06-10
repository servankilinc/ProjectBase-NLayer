using Business.Abstract;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.User_;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService) => _userService = userService;

    #region GetBasic
    [HttpGet]
    public async Task<IActionResult> Get(Guid Id)
    {
        var result = await _userService.GetAsync(Id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetAll(DynamicRequest? request)
    {
        var result = await _userService.GetAllAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetList(DynamicPaginationRequest request)
    {
        var result = await _userService.GetListAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region GetDetail
    [HttpGet]
    public async Task<IActionResult> GetByDetail(Guid Id)
    {
        var result = await _userService.GetByDetailAsync(Id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetAllByDetail(DynamicRequest? request)
    {
        var result = await _userService.GetAllByDetailAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetListByDetail(DynamicPaginationRequest request)
    {
        var result = await _userService.GetListByDetailAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region Get-UserBlogsResponseDto
    [HttpGet]
    public async Task<IActionResult> GetUserBlogsResponseDto(Guid Id)
    {
        var result = await _userService.GetUserBlogsResponseDtoAsync(Id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetAllUserBlogsResponseDto(DynamicRequest? request)
    {
        var result = await _userService.GetAllUserBlogsResponseDtoAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetListUserBlogsResponseDto(DynamicPaginationRequest request)
    {
        var result = await _userService.GetListUserBlogsResponseDtoAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create(UserCreateDto request)
    {
        var result = await _userService.CreateAsync(request);

        return Ok(result);
    }
    #endregion

    #region Update
    [HttpPatch]
    public async Task<IActionResult> Update(UserUpdateDto request)
    {
        var result = await _userService.UpdateAsync(request);

        return Ok(result);
    }
    #endregion

    #region Delete
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid Id)
    {
        await _userService.DeleteAsync(Id);

        return Ok();
    }
    #endregion

    #region Datatable Methods
    [HttpPost]
    public async Task<IActionResult> DatatableClientSide(DynamicRequest request)
    {
        var result = await _userService.DatatableClientSideAsync(request);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableServerSideRequest request)
    {
        var result = await _userService.DatatableServerSideAsync(request);

        return Ok(result);
    }
    #endregion
}
