using System.Runtime.CompilerServices;
using Application.DTO;
using Application.Mappers;
using Application.XuLyNews.Requests;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.XuLyNews.UsesCases
{
    
    public class GetAllNewsUseCase : IRequestHandler<GetAllNewsRequest, IAsyncEnumerable<NewsResponseDto>>
    {
        private readonly INewsRepo _newsRepo;

        public GetAllNewsUseCase(INewsRepo newsRepo)
        {
            _newsRepo = newsRepo;
        }

        public async Task<IAsyncEnumerable<NewsResponseDto>> Handle(GetAllNewsRequest request, CancellationToken cancellationToken)
        {
                 return StreamNews(cancellationToken);
        }
        private async IAsyncEnumerable<NewsResponseDto> StreamNews([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var n in _newsRepo.GetAllWithMenusAsync().WithCancellation(cancellationToken))
            {
                var dto = n.ToDto();
                    dto.Menus = n.Menu
                        .Where(m => !m.is_deleted)
                        .Select(m => new MenuBasicResponseDto
                        {
                            Id   = m.Id,
                            Name = m.Name ?? string.Empty,
                            Slug = m.Slug ?? string.Empty
                        });
                    yield return dto;
            }
        }
    }
}
