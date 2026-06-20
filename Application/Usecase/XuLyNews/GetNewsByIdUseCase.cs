using Application.DTO;
using Application.Requests.XuLyNews;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyNews
{
    public class GetNewsByIdUseCase : IRequestHandler<GetNewsByIdRequest, NewsResponseDto?>
    {
        private readonly INewsQueryRepository _newsRepo;
        public GetNewsByIdUseCase(INewsQueryRepository newsRepo)
        {
            _newsRepo = newsRepo;
        }
        public async Task<NewsResponseDto?> Handle(GetNewsByIdRequest request, CancellationToken cancellationToken)
        {
                return await _newsRepo.GetByIdWithMenusAsync(request.id);
           
    }
}
}