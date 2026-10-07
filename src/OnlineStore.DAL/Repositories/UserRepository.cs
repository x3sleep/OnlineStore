using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Data;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Repositories.Interfaces;

namespace OnlineStore.DAL.Repositories;

public class UserRepository(StoreDbContext context) : GenericRepository<User>(context), IUserRepository
{
    public Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken = default) =>
        Set.FirstOrDefaultAsync(u => u.Login == login, cancellationToken);

    public Task<bool> LoginExistsAsync(string login, int? exceptUserId = null, CancellationToken cancellationToken = default) =>
        Set.AnyAsync(u => u.Login == login && u.Id != exceptUserId, cancellationToken);
}
