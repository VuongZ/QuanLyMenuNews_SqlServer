using MediatR;

namespace Application.Requests.XuLyMenu
{
    public class UpdateMenuRequest :IRequest<bool>
    {
        public int id { get; set; }
         public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
  
    }
}