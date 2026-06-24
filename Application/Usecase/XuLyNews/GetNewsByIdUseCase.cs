using Application.DTO;
using Application.Mappers;
using Application.Requests.XuLyNews;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyNews
{
    public class GetNewsByIdUseCase : IRequestHandler<GetNewsByIdRequest, NewsResponseDto?>
    {
        private readonly INewsRepo _newsRepo;
        public GetNewsByIdUseCase(INewsRepo newsRepo)
        {
            _newsRepo = newsRepo;
        }
        public async Task<NewsResponseDto?> Handle(GetNewsByIdRequest request,CancellationToken cancellationToken)
        {
            var n = await _newsRepo.GetByIdWithMenusAsync(request.id);
            if (n == null) return null;
                var dto = n.ToDto();
                    dto.Menus = n.Menu
                        .Where(m => !m.is_deleted)
                        .Select(m => new MenuBasicResponseDto
                        {
                            Id   = m.Id,
                            Name = m.Name ?? string.Empty,
                            Slug = m.Slug ?? string.Empty
                        });
                    return dto;
         
        }
}
}