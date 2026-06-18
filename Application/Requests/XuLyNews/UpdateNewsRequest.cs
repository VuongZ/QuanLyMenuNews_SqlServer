using MediatR;

namespace Application.Requests.XuLyNews
{
    public class UpdateNewsRequest : IRequest<bool>
    {
        public int id { get; set; }
        public string ? title { get; set; } 
        public string  slug { get; set; }= string.Empty;
        public string ? content { get; set; }
        public string ? thumbnail { get; set; }
    }
}