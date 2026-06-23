using System.Runtime.CompilerServices;
using Application.DTO;
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
            await foreach (var n in _newsRepo.GetAllWithMenusAsync()
                               .WithCancellation(cancellationToken))
            {
                yield return new NewsResponseDto
                {
                    Id        = n.Id,
                    Title     = n.Title     ?? string.Empty,
                    Slug      = n.Slug      ?? string.Empty,
                    Content   = n.Content,
                    Thumbnail = n.thumbnail,
                    Address   = n.Address,
                    WardId    = n.WardId,

                    FullAddress = n.Ward == null
                        ? n.Address
                        : n.Address + ", " + n.Ward.FullName,

                    WardInfo = n.Ward == null ? null : new WardInfoResponseDto
                    {
                        WardId     = n.Ward.WardId,
                        WardPid    = n.Ward.WardPid,
                        Name       = n.Ward.Name       ?? string.Empty,
                        NameEn     = n.Ward.NameEn,
                        FullName   = n.Ward.FullName   ?? string.Empty,
                        FullNameEn = n.Ward.FullNameEn,
                        Country    = n.Ward.Localization?.Localization ?? string.Empty,
                        WardParent = new WardParentResponseDto
                        {
                            WardId = n.Ward.WardPid,
                            Name   = n.Ward.FullName != null && n.Ward.FullName.Contains(",")
                                ? n.Ward.FullName.Substring(n.Ward.FullName.IndexOf(",") + 1).Trim()
                                : string.Empty,
                            NameEn = n.Ward.FullNameEn != null && n.Ward.FullNameEn.Contains(",")
                                ? n.Ward.FullNameEn.Substring(n.Ward.FullNameEn.IndexOf(",") + 1).Trim()
                                : null,
                            Country = n.Ward.Localization?.Localization ?? string.Empty,
                        }
                    },

                    Menus = n.Menu.Select(m => new MenuBasicResponseDto
                    {
                        Id   = m.Id,
                        Name = m.Name ?? string.Empty,
                        Slug = m.Slug ?? string.Empty,
                    })
                };
            }
        }
            }
        }
