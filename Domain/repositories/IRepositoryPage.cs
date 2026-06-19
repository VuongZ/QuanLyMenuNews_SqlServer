using Domain.entity;

namespace Domain.repositories;
public interface IRepository<T> where T : BaseId
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task SoftDelete(int id);
     Task<bool> ExistsAsync(int id);
     Task<int> SoftDeleteManyAsync(IEnumerable<int> ids);
}