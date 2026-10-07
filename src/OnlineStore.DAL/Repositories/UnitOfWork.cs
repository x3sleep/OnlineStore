using Microsoft.EntityFrameworkCore.Storage;
using OnlineStore.DAL.Data;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Repositories.Interfaces;

namespace OnlineStore.DAL.Repositories;

public class UnitOfWork(StoreDbContext context) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;
    private IRepository<Category>? _categories;
    private IRepository<Brand>? _brands;
    private IRepository<Address>? _addresses;
    private IRepository<Payment>? _payments;
    private IProductRepository? _products;
    private ICustomerRepository? _customers;
    private IOrderRepository? _orders;
    private IUserRepository? _users;

    public IRepository<Category> Categories => _categories ??= new GenericRepository<Category>(context);
    public IRepository<Brand> Brands => _brands ??= new GenericRepository<Brand>(context);
    public IRepository<Address> Addresses => _addresses ??= new GenericRepository<Address>(context);
    public IRepository<Payment> Payments => _payments ??= new GenericRepository<Payment>(context);
    public IProductRepository Products => _products ??= new ProductRepository(context);
    public ICustomerRepository Customers => _customers ??= new CustomerRepository(context);
    public IOrderRepository Orders => _orders ??= new OrderRepository(context);
    public IUserRepository Users => _users ??= new UserRepository(context);

    public int SaveChanges() => context.SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("Транзакция не начата");
        }

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            return;
        }

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
        context.ChangeTracker.Clear();
    }
}
