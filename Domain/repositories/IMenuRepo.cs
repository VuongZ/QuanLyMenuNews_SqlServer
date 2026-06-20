using Domain.entity;

namespace Domain.repositories
{
    public interface IMenuRepo : IRepository<Menu>
    {
        Task<Menu?> GetBySlugAsync(string slug); 
        Task<Menu?> GetByIdWithNewsForUpdateAsync(int id);

    }   
}