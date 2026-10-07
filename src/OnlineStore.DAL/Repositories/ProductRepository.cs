using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Data;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Repositories.Interfaces;

namespace OnlineStore.DAL.Repositories;

public class ProductRepository(StoreDbContext context) : GenericRepository<Product>(context), IProductRepository
{
    public Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default) =>
        Set.FirstOrDefaultAsync(p => p.Sku == sku, cancellationToken);

    public async Task<IReadOnlyList<Product>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default) =>
        await Set.AsNoTracking()
            .Where(p => p.CategoryId == categoryId || p.Category.ParentId == categoryId)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default) =>
        await Set.AsNoTracking()
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
            .OrderBy(p => p.Price)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Product>> SearchByNameAsync(string text, CancellationToken cancellationToken = default) =>
        await Set.AsNoTracking()
            .Where(p => EF.Functions.Like(p.Name, $"%{text}%"))
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
}
