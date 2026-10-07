using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Data;
using OnlineStore.DAL.Repositories.Interfaces;

namespace OnlineStore.DAL.Repositories;

public class GenericRepository<T>(StoreDbContext context) : IRepository<T> where T : class
{
    protected StoreDbContext Context { get; } = context;
    protected DbSet<T> Set { get; } = context.Set<T>();

    public T? GetById(int id) => Set.Find(id);

    public IEnumerable<T> GetAll() => Set.AsNoTracking().ToList();

    public void Add(T entity) => Set.Add(entity);

    public void Update(T entity) => Set.Update(entity);

    public void Delete(int id)
    {
        var entity = Set.Find(id);
        if (entity is not null)
        {
            Set.Remove(entity);
        }
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await Set.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await Set.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await Set.AddAsync(entity, cancellationToken);

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await Set.FindAsync([id], cancellationToken);
        if (entity is not null)
        {
            Set.Remove(entity);
        }
    }
}
