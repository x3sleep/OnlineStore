using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Data;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Repositories.Interfaces;

namespace OnlineStore.DAL.Repositories;

public class CustomerRepository(StoreDbContext context) : GenericRepository<Customer>(context), ICustomerRepository
{
    public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        Set.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);

    public Task<Customer?> GetWithAddressesAsync(int id, CancellationToken cancellationToken = default) =>
        Set.Include(c => c.Addresses).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
}
