using SecurityPlatform.Domain.Enums;

namespace SecurityPlatform.Domain.Entities;

public sealed class User
{
    private readonly List<Role> _roles = new();

    private User()
    {
    }

    public User(
        Guid id,
        string username,
        string email)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username is required.", nameof(username));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(email));
        }

        Id = id;
        Username = username.Trim();
        Email = email.Trim();
        Status = AccountStatus.Active;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public string Username { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public AccountStatus Status { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    public void AssignRole(Role role)
    {
        ArgumentNullException.ThrowIfNull(role);

        if (_roles.Any(existing => existing.Id == role.Id))
        {
            return;
        }

        _roles.Add(role);
    }

    public void RemoveRole(Guid roleId)
    {
        _roles.RemoveAll(role => role.Id == roleId);
    }

    public void Disable()
    {
        Status = AccountStatus.Disabled;
    }

    public void Activate()
    {
        Status = AccountStatus.Active;
    }

    public void Lock()
    {
        Status = AccountStatus.Locked;
    }
}
