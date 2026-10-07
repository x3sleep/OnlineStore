using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken = default);
    Task<bool> LoginExistsAsync(string login, int? exceptUserId = null, CancellationToken cancellationToken = default);
}
