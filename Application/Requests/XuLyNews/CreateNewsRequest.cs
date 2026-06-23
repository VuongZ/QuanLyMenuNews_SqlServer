using Application.Requests.XuLyMenu;
using MediatR;

namespace Application.Requests.XuLyNews
{
public class CreateNewsRequest : IRequest<bool>
{
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Thumbnail { get; set; } = null!;

    public string? Address { get; set; }
    
     public int? ProvinceId { get; set; }  
    public int? WardId { get; set; }
    public List<MenuInputRequest> DanhSachMenus { get; set; } = new();
}
 public class MenuInputRequest
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
    }
}