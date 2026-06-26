using Domain.entity;

namespace Domain.repositories
{
    public interface INewsRepo : IRepository<News>
    {
        Task<News?> GetBySlugAsync(string slug);
        
        IQueryable<News> GetByIdWithMenusAsync(int id);
        IQueryable<News> GetAllWithMenusAsync();

    }
}