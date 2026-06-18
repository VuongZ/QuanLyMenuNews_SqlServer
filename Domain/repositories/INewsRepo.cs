using Domain.entity;

namespace Domain.repositories
{
    public interface INewsRepo : IRepository<News>
    {
         Task<News?> GetBySlugAsync(string slug);
        Task<News?> GetByIdWithMenusAsync(int id);
        Task<IEnumerable<News>> GetAllWithMenusAsync();
    }
}