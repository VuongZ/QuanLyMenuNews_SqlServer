using Application.Requests.XuLyMenu;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller;
[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
        private readonly IMediator _mediator;

    public MenuController( IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllMenuRequest());
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetMenuByIdRequest { id = id });
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateMenuRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
     [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateMenuRequest request)
    {
        request.id = id;
        var result = await _mediator.Send(request);
        if (!result) return NotFound();
        return Ok(result);
    }
      [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteMenuRequest { Id = id });
        if (!result) return NotFound();
        return Ok(result);
    }

}