using Domain.entity;

namespace Domain.repositories
{
    public interface IMenuNewsRepo
    {
        IQueryable<MenuNews> GetAllAsync();
        Task<IEnumerable<MenuNews>> SearchAsync(int? menuId, int? newsId);
        IQueryable<MenuNews> GetByIdAsync(int menuId, int newsId);
        Task AddAsync(MenuNews menuNews);
        Task UpdateAsync(MenuNews menuNews);
        Task DeleteAsync(int menuId, int newsId);
        Task<bool> ExistsAsync(int menuId, int newsId);
        Task<List<int>> GetNewsIdsByMenuIdAsync(int menuId,CancellationToken cancellationToken = default);
        Task RemoveByMenuAndNewsIdsAsync(int menuId,IEnumerable<int> newsIds,CancellationToken cancellationToken = default);
        Task<List<int>> GetMenuIdsByNewsIdAsync(int newsId,CancellationToken cancellationToken = default);
        Task RemoveByNewsAndMenuIdsAsync(int newsId,IEnumerable<int> menuIds,CancellationToken cancellationToken = default);
    }
}
