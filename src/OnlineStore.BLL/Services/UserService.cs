using OnlineStore.BLL.DTOs;
using OnlineStore.BLL.Exceptions;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Repositories.Interfaces;

namespace OnlineStore.BLL.Services;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
    public async Task<UserResponse> CreateAsync(UserRequest request, CancellationToken cancellationToken = default)
    {
        var login = request.Login.Trim();
        if (await unitOfWork.Users.LoginExistsAsync(login, cancellationToken: cancellationToken))
        {
            throw new ConflictException($"Логин «{login}» уже занят");
        }

        var user = new User { Login = login, PassHash = request.PassHash.ToLowerInvariant() };
        await unitOfWork.Users.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ToResponse(user);
    }

    public async Task<UserResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        ToResponse(await FindAsync(id, cancellationToken));

    public async Task<UserResponse> UpdateAsync(int id, UserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await FindAsync(id, cancellationToken);
        var login = request.Login.Trim();
        if (await unitOfWork.Users.LoginExistsAsync(login, id, cancellationToken))
        {
            throw new ConflictException($"Логин «{login}» уже занят");
        }

        user.Login = login;
        user.PassHash = request.PassHash.ToLowerInvariant();
        unitOfWork.Users.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ToResponse(user);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await FindAsync(id, cancellationToken);
        await unitOfWork.Users.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<User> FindAsync(int id, CancellationToken cancellationToken) =>
        await unitOfWork.Users.GetByIdAsync(id, cancellationToken)
        ?? throw new NotFoundException($"Пользователь с Id = {id} не найден");

    private static UserResponse ToResponse(User user) => new(user.Id, user.Login);
}
