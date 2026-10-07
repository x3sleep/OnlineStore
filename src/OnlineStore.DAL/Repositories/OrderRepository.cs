using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Data;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Repositories.Interfaces;

namespace OnlineStore.DAL.Repositories;

public class OrderRepository(StoreDbContext context) : GenericRepository<Order>(context), IOrderRepository
{
    public Task<Order?> GetWithItemsAsync(int id, CancellationToken cancellationToken = default) =>
        Set.Include(o => o.Items).ThenInclude(i => i.Product)
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Order>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken = default) =>
        await Set.AsNoTracking()
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default) =>
        await Set.AsNoTracking()
            .Where(o => o.Status == status)
            .ToListAsync(cancellationToken);
}
