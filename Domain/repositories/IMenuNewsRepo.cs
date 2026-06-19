using Domain.entity;

namespace Domain.repositories
{
    public interface IMenuNewsRepo
    {
        Task<IEnumerable<MenuNews>> GetAllAsync();
        Task<IEnumerable<MenuNews>> GetById(int? menuId, int? newsId);
        Task<MenuNews?> GetByIdAsync(int menuId, int newsId);
        Task AddAsync(MenuNews menuNews);
        Task UpdateAsync(MenuNews menuNews);
        Task DeleteAsync(int menuId, int newsId);
    }
}
