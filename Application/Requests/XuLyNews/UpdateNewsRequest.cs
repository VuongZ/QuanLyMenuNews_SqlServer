using MediatR;

namespace Application.Requests.XuLyNews
{
    public class UpdateNewsRequest : IRequest<bool>
    {
        public int id { get; set; }
        public string ? title { get; set; } 
        public string  slug { get; set; }= string.Empty;
        public string ? content { get; set; }
        public string ? Thumbnail { get; set; }
        public string? CountryKey { get; set; }

        public int? ProvinceId { get; set; }
        public int? WardId { get; set; }
        public string? Address { get; set; }
          public List<UpdateNewsMenuItemRequest> DanhSachMenus { get; set; }= new();
    }
    public class UpdateNewsMenuItemRequest
    {
    public int? Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;
}
}