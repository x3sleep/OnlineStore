using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.Repositories.Interfaces;

public interface IUnitOfWork
{
    IRepository<Category> Categories { get; }
    IRepository<Brand> Brands { get; }
    IRepository<Address> Addresses { get; }
    IRepository<Payment> Payments { get; }
    IProductRepository Products { get; }
    ICustomerRepository Customers { get; }
    IOrderRepository Orders { get; }
    IUserRepository Users { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
