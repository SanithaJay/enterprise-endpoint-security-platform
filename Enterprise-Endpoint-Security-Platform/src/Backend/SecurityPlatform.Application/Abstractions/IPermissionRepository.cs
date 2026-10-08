using SecurityPlatform.Domain.Entities;

namespace SecurityPlatform.Application.Abstractions;

public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(
        Guid permissionId,
        CancellationToken cancellationToken = default);

    Task<Permission?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Permission permission,
        CancellationToken cancellationToken = default);
}
