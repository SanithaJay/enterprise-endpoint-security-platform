namespace SecurityPlatform.Domain.Entities;

public sealed class Permission
{
    private Permission()
    {
    }

    public Permission(
        Guid id,
        string name,
        string description)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Permission ID cannot be empty.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Permission name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Permission description is required.", nameof(description));
        }

        Id = id;
        Name = name.Trim();
        Description = description.Trim();
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;
}
