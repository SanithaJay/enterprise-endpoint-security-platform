using SecurityPlatform.Domain.Entities;

namespace SecurityPlatform.Application.Abstractions;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);

    Task<Role?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Role role,
        CancellationToken cancellationToken = default);
}
