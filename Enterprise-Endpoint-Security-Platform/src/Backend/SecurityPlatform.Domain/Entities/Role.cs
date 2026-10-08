namespace SecurityPlatform.Domain.Entities;

public sealed class Role
{
    private readonly List<Permission> _permissions = new();

    private Role()
    {
    }

    public Role(
        Guid id,
        string name)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Role ID cannot be empty.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Role name is required.", nameof(name));
        }

        Id = id;
        Name = name.Trim();
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

    public void GrantPermission(Permission permission)
    {
        ArgumentNullException.ThrowIfNull(permission);

        if (_permissions.Any(existing => existing.Id == permission.Id))
        {
            return;
        }

        _permissions.Add(permission);
    }

    public void RevokePermission(Guid permissionId)
    {
        _permissions.RemoveAll(permission => permission.Id == permissionId);
    }
}
