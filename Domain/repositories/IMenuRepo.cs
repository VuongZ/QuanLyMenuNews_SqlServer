using Domain.entity;

namespace Domain.repositories
{
    public interface IMenuRepo : IRepository<Menu>
    {
        Task<Menu?> GetBySlugAsync(string slug); 
        IQueryable<Menu> GetByIdWithNewsAsync(int id);
        IQueryable<Menu> GetAllWithNewsAsync();  


    }   
}