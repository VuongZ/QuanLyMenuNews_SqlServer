using Application.DTO;
using Application.XuLyNews.Requests;
using Domain.repositories;
using MediatR;

namespace Application.XuLyNews.UsesCases
{
    
    public class GetNewsDaXoaUseCase : IRequestHandler<GetNewsDaXoaRequest, IEnumerable<NewsBasicResponseDTO>>
    {
        private readonly INewsRepo newsRepo;

        public GetNewsDaXoaUseCase(INewsRepo Repo)
        {
            newsRepo = Repo;
        }

        public  async  Task<IEnumerable<NewsBasicResponseDTO>> Handle(GetNewsDaXoaRequest request, CancellationToken cancellationToken)
        {
            var news= await newsRepo.GetDaXoa();
            return news.Select(n => new NewsBasicResponseDTO{
                    Id        = n.Id,
                    Title     = n.Title,
                    Slug      = n.Slug,
                    Content   = n.Content,
                    Thumbnail = n.Thumbnail,
                    Address   = n.Address,
                    WardId    = n.WardId,
        }).AsEnumerable();
        }
    }
}
