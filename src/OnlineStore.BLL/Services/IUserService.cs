using OnlineStore.BLL.DTOs;

namespace OnlineStore.BLL.Services;

public interface IUserService
{
    Task<UserResponse> CreateAsync(UserRequest request, CancellationToken cancellationToken = default);
    Task<UserResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserResponse> UpdateAsync(int id, UserRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
