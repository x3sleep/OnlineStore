using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.Repositories.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Customer?> GetWithAddressesAsync(int id, CancellationToken cancellationToken = default);
}
