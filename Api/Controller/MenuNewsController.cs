using Application.Requests.XuLyMenuNews;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class MenuNewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MenuNewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllMenuNewsRequest());
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] int? menuId, [FromQuery] int? newsId)
    {
        var result = await _mediator.Send(new SearchMenuNewsRequest
        {
            MenuId = menuId,
            NewsId = newsId
        });
        return Ok(result);
    }

    [HttpGet("{menuId}/{newsId}")]
    public async Task<IActionResult> GetById(int menuId, int newsId)
    {
        var result = await _mediator.Send(new GetMenuNewsByIdRequest
        {
            MenuId = menuId,
            NewsId = newsId
        });

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMenuNewsRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPut("{menuId}/{newsId}")]
    public async Task<IActionResult> Update(int menuId, int newsId, UpdateMenuNewsRequest request)
    {
        request.OldMenuId = menuId;
        request.OldNewsId = newsId;

        var result = await _mediator.Send(request);
        if (!result)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{menuId}/{newsId}")]
    public async Task<IActionResult> Delete(int menuId, int newsId)
    {
        var result = await _mediator.Send(new DeleteMenuNewsRequest
        {
            MenuId = menuId,
            NewsId = newsId
        });

        if (!result)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
