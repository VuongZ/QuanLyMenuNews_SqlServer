using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.XuLyNews.Requests;
using Application.Requests.XuLyNews;

namespace Api.Controller;
[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly IMediator _mediator;
    public NewsController(IMediator mediator)
    {
        _mediator=mediator;
    }
[HttpGet]
public async Task<IActionResult> GetAllNews()
{
    var news = await _mediator.Send(new GetAllNewsRequest());

    return Ok(news);
}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNewsById(int id)
    {
        var news = await _mediator.Send(new GetNewsByIdRequest { id = id });
        if (news == null)
        {
            return NotFound();
        }
        return Ok(news);
    }
    [HttpPost]
    public async Task<IActionResult> CreateNews(CreateNewsRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
     [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNews(int id, UpdateNewsRequest request)
    {
        request.id = id;
        var result = await _mediator.Send(request);
        if (!result) return NotFound();
        return Ok(result);
    }
      [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNews(int id)
    {
        var result = await _mediator.Send(new DeleteNewsRequest { id = id });
        if (!result) return NotFound();
        return Ok(result);
    }
    [HttpDelete("delete-many")]
    public async Task<IActionResult> DeleteManyNews(
        [FromBody] DeleteManyNewsRequest request)
    {
        var deletedCount = await _mediator.Send(request);

        return Ok(new
        {
            message = "Xóa nhiều News thành công.",
            deletedCount
        });
    }
}