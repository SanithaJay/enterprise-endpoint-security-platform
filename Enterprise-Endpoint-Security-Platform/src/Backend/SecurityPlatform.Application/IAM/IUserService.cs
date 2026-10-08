using SecurityPlatform.Domain.Entities;

namespace SecurityPlatform.Application.IAM;

public interface IUserService
{
    Task<User> CreateUserAsync(
        string username,
        string email,
        CancellationToken cancellationToken = default);

    Task<User?> GetUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task AssignRoleAsync(
        Guid userId,
        Guid roleId,
        CancellationToken cancellationToken = default);
}
