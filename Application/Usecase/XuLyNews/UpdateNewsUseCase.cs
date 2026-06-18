using Domain.repositories;
using FluentValidation;
using MediatR;

namespace Application.Requests.XuLyNews
{
    public class UpdateNewsUseCase : IRequestHandler<UpdateNewsRequest, bool>
    {
        private readonly INewsRepo _newsRepo;
        private readonly IUnitOfWork _uow;
        public UpdateNewsUseCase(INewsRepo newsRepo, IUnitOfWork uow)
        {
            _newsRepo = newsRepo;
            _uow = uow;
        }
        public async Task<bool> Handle(UpdateNewsRequest request, CancellationToken cancellationToken)
        {
            var existing = await _newsRepo.GetBySlugAsync(request.slug.Trim().ToLower());

             if (existing != null && existing.Id != request.id)
          {
            throw new ValidationException("Slug menu đã tồn tại.");
             }
            var news = await _newsRepo.GetByIdAsync(request.id);
            if (news == null)
                return false;

            news.Title = request.title;
            news.Content = request.content;
            news.Slug = request.slug.Trim().ToLower();
            news.thumbnail = request.thumbnail;
            news.updated_at = DateTime.UtcNow;

            await _newsRepo.UpdateAsync(news);
            await _uow.SaveChangesAsync();
            return true;
    }
}
}