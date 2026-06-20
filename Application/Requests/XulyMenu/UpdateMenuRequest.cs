using MediatR;

namespace Application.Requests.XuLyMenu;

public class UpdateMenuRequest : IRequest<bool>
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public List<UpdateMenuNewsItemRequest> DanhSachNews { get; set; } = new();
}

public class UpdateMenuNewsItemRequest
{
    public int? Id { get; set; }

    public string Title { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Thumbnail { get; set; }
    public string? ProvinceName { get; set; }

    public string? WardName { get; set; }

    public string? Address { get; set; }
}