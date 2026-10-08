using SecurityPlatform.Application.Abstractions;
using SecurityPlatform.Domain.Entities;

namespace SecurityPlatform.Application.IAM;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<User> CreateUserAsync(
        string username,
        string email,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException(
                "Username is required.",
                nameof(username));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(
                "Email is required.",
                nameof(email));
        }

        var existingUser = await _userRepository.GetByUsernameAsync(
            username.Trim(),
            cancellationToken);

        if (existingUser is not null)
        {
            throw new InvalidOperationException(
                $"A user with username '{username.Trim()}' already exists.");
        }

        var user = new User(
            Guid.NewGuid(),
            username,
            email);

        await _userRepository.AddAsync(
            user,
            cancellationToken);

        return user;
    }

    public Task<User?> GetUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID cannot be empty.",
                nameof(userId));
        }

        return _userRepository.GetByIdAsync(
            userId,
            cancellationToken);
    }

    public async Task AssignRoleAsync(
        Guid userId,
        Guid roleId,
        CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID cannot be empty.",
                nameof(userId));
        }

        if (roleId == Guid.Empty)
        {
            throw new ArgumentException(
                "Role ID cannot be empty.",
                nameof(roleId));
        }

        var user = await _userRepository.GetByIdAsync(
            userId,
            cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException(
                $"User '{userId}' was not found.");
        }

        var role = await _roleRepository.GetByIdAsync(
            roleId,
            cancellationToken);

        if (role is null)
        {
            throw new KeyNotFoundException(
                $"Role '{roleId}' was not found.");
        }

        user.AssignRole(role);

        await _userRepository.UpdateAsync(
            user,
            cancellationToken);
    }
}
