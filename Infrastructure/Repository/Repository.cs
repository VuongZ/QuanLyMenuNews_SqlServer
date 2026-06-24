using Microsoft.EntityFrameworkCore;
using Domain.entity;
using Domain.repositories;
using Infrastructure.Data;

namespace Infrastructure.Repository;
public abstract class Repository<T> : IRepository<T> where T : BaseId
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;
    public Repository (AppDbContext appContext)
    {
        _context=appContext;
        _dbSet=appContext.Set<T>();
    }
   public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.Where(e=> e.is_deleted==false).ToListAsync();
    }
    
    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
 
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
   
    }

    public async Task<T?> GetByIdAsync(int id)
    {
       return await _dbSet.FirstOrDefaultAsync(e=>e.Id==id && e.is_deleted==false);
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if(entity == null)
        {
            return;
        }
        _dbSet.Remove(entity);
       
    }
    public async Task<bool> ExistsAsync(int id)
{
      var entity = await _dbSet
            .FirstOrDefaultAsync(x => x.Id == id  && !x.is_deleted);

        if (entity == null)
        {
            return false;
        }

        return true;
}

    public async Task SoftDelete(int id)
    {
         var entity = await GetByIdAsync(id);
        if(entity == null)
        {
            return;
        }
        entity.is_deleted=true;
        entity.deleted_at=DateTime.UtcNow;
        _dbSet.Update(entity);

    }
    public async Task<int> SoftDeleteManyAsync(IEnumerable<int> ids)
{
    var distinctIds = ids.Distinct().ToList();

    var entities = await _dbSet
        .Where(x =>
            distinctIds.Contains(x.Id) &&
            !x.is_deleted)
        .ToListAsync();

    var now = DateTime.UtcNow;

    foreach (var entity in entities)
    {
        entity.is_deleted = true;
        entity.deleted_at = now;
        entity.updated_at = now;
    }

    return entities.Count;
}
}
