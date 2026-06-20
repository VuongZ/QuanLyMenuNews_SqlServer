using Application.DTO;
using Application.XuLyNews.Requests;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.XuLyNews.UsesCases
{
    
    public class GetAllNewsUseCase : IRequestHandler<GetAllNewsRequest, IAsyncEnumerable<NewsResponseDto>>
    {
        private readonly INewsQueryRepository _newsRepo;

        public GetAllNewsUseCase(INewsQueryRepository newsRepo)
        {
            _newsRepo = newsRepo;
        }

        public async Task<IAsyncEnumerable<NewsResponseDto>> Handle(GetAllNewsRequest request, CancellationToken cancellationToken)
        {
            return  _newsRepo.GetAllWithMenusAsync();
           
        }
    }
}