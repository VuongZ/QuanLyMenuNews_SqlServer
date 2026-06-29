using Application.Requests.XuLyMenu;
using Application.XuLyMenu.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller;
[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMediator mediator;

    public MenuController( IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetAllMenuRequest());

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await mediator.Send(new GetMenuByIdRequest { id = id });
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateMenuRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,UpdateMenuRequest request)
    {
    request.Id = id;
    var result = await mediator.Send(request);
    if (!result)
    {
        return NotFound();
    }
    return Ok(new
    {
            message = "Cập nhật Menu và danh sách News thành công."
    });
}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await this.mediator.Send(new DeleteMenuRequest { Id = id });
        if (!result) return NotFound();
        return Ok(result);
    }
    [HttpDelete("delete-many")]
    public async Task<IActionResult> DeleteMany([FromBody] DeleteManyMenuRequest request)
    {
        var deletedCount = await this.mediator.Send(request);
        return Ok(new
        {
            message = "Xóa nhiều Menu thành công.",
            deletedCount
        });
    }
    [HttpGet("menu-da-xoa")]
    public async Task<IActionResult> GetDaXoa()
    {
        var result = await this.mediator.Send(new GetMenuDaXoaRequest());
        return Ok(result);
    }
    [HttpPatch("{id}/restore")]
    public async Task<IActionResult> RestoreNews(int id)
    {
        var result = await mediator.Send(new RestoreMenuRequest { Id = id });
        return Ok(result);
    }
}