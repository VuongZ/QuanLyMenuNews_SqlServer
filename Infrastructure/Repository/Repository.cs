using Microsoft.EntityFrameworkCore;
using Domain.entity;
using Domain.repositories;
using Infrastructure.Data;

namespace Infrastructure.Repository;
public abstract class Repository<T> : IRepository<T> where T : BaseId
{
    protected readonly AppDbContext context;
    protected readonly DbSet<T> dbset;
    public Repository (AppDbContext appContext)
    {
        context=appContext;
        dbset=appContext.Set<T>();
    }
   public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await dbset.Where(e=> e.IsDeleted==false).ToListAsync();
    }
    
    public async Task<T> AddAsync(T entity)
    {
        await dbset.AddAsync(entity);

        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        dbset.Update(entity);

    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await dbset.FirstOrDefaultAsync(e=>e.Id==id && e.IsDeleted==false);
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if(entity == null)
        {
            return;
        }
        dbset.Remove(entity);
    }
    public async Task<bool> ExistsAsync(int id)
{
    var entity = await dbset.FirstOrDefaultAsync(x => x.Id == id  && !x.IsDeleted);
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
        entity.IsDeleted=true;
        entity.DateledAt=DateTime.UtcNow;
        dbset.Update(entity);

    }
    public async Task<int> SoftDeleteManyAsync(IEnumerable<int> ids)
{
    var distinctIds = ids.Distinct().ToList();
    var entities = await dbset
        .Where(x =>distinctIds.Contains(x.Id) &&!x.IsDeleted)
        .ToListAsync();
    var now = DateTime.UtcNow;
    foreach (var entity in entities)
    {
        entity.IsDeleted = true;
        entity.DateledAt = now;
        entity.UpdatedAt = now;
    }
    return entities.Count;
}
}
