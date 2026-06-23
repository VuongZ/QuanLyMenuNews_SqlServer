using MediatR;

namespace Application.Requests.XuLyMenu
{
    public class CreateMenuRequest : IRequest<bool>
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public List<NewsInputRequest> DanhSachNews { get; set; } = new();
     
    }
       public class NewsInputRequest
    {
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Thumbnail { get; set; } = null!;
        public int? ProvinceId { get; set; }
        public int? WardId { get; set; } 

        public string? Address { get; set; }
    }
    
}