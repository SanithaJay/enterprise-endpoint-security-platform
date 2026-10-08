using SecurityPlatform.Application.Abstractions;
using SecurityPlatform.Application.IAM;
using SecurityPlatform.Domain.Entities;

namespace SecurityPlatform.UnitTests.Application.IAM;

public class UserServiceTests
{
    [Fact]
    public async Task CreateUserAsync_ShouldCreateAndPersistUser()
    {
        var userRepository = new FakeUserRepository();
        var roleRepository = new FakeRoleRepository();

        var service = new UserService(
            userRepository,
            roleRepository);

        var user = await service.CreateUserAsync(
            "sanitha",
            "sanitha@example.com");

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("sanitha", user.Username);
        Assert.Equal("sanitha@example.com", user.Email);
        Assert.Same(user, userRepository.StoredUser);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldRejectDuplicateUsername()
    {
        var existingUser = new User(
            Guid.NewGuid(),
            "sanitha",
            "sanitha@example.com");

        var userRepository = new FakeUserRepository
        {
            StoredUser = existingUser
        };

        var roleRepository = new FakeRoleRepository();

        var service = new UserService(
            userRepository,
            roleRepository);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateUserAsync(
                "sanitha",
                "another@example.com"));

        Assert.Contains("already exists", exception.Message);
    }

    [Fact]
    public async Task GetUserAsync_ShouldReturnUser()
    {
        var user = new User(
            Guid.NewGuid(),
            "sanitha",
            "sanitha@example.com");

        var userRepository = new FakeUserRepository
        {
            StoredUser = user
        };

        var roleRepository = new FakeRoleRepository();

        var service = new UserService(
            userRepository,
            roleRepository);

        var result = await service.GetUserAsync(user.Id);

        Assert.Same(user, result);
    }

    [Fact]
    public async Task GetUserAsync_ShouldRejectEmptyUserId()
    {
        var service = new UserService(
            new FakeUserRepository(),
            new FakeRoleRepository());

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.GetUserAsync(Guid.Empty));
    }

    [Fact]
    public async Task AssignRoleAsync_ShouldAssignRoleAndPersistUser()
    {
        var user = new User(
            Guid.NewGuid(),
            "sanitha",
            "sanitha@example.com");

        var role = new Role(
            Guid.NewGuid(),
            "Security Admin");

        var userRepository = new FakeUserRepository
        {
            StoredUser = user
        };

        var roleRepository = new FakeRoleRepository
        {
            StoredRole = role
        };

        var service = new UserService(
            userRepository,
            roleRepository);

        await service.AssignRoleAsync(
            user.Id,
            role.Id);

        Assert.Single(user.Roles);
        Assert.Contains(role, user.Roles);
        Assert.Same(user, userRepository.UpdatedUser);
    }

    [Fact]
    public async Task AssignRoleAsync_ShouldRejectMissingUser()
    {
        var userRepository = new FakeUserRepository();
        var roleRepository = new FakeRoleRepository
        {
            StoredRole = new Role(
                Guid.NewGuid(),
                "Security Admin")
        };

        var service = new UserService(
            userRepository,
            roleRepository);

        var userId = Guid.NewGuid();

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.AssignRoleAsync(
                userId,
                roleRepository.StoredRole!.Id));

        Assert.Contains("was not found", exception.Message);
    }

    [Fact]
    public async Task AssignRoleAsync_ShouldRejectMissingRole()
    {
        var user = new User(
            Guid.NewGuid(),
            "sanitha",
            "sanitha@example.com");

        var userRepository = new FakeUserRepository
        {
            StoredUser = user
        };

        var service = new UserService(
            userRepository,
            new FakeRoleRepository());

        var roleId = Guid.NewGuid();

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.AssignRoleAsync(
                user.Id,
                roleId));

        Assert.Contains("was not found", exception.Message);
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        public User? StoredUser { get; set; }

        public User? UpdatedUser { get; private set; }

        public Task<User?> GetByIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                StoredUser?.Id == userId ? StoredUser : null);
        }

        public Task<User?> GetByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                StoredUser is not null &&
                string.Equals(
                    StoredUser.Username,
                    username,
                    StringComparison.OrdinalIgnoreCase)
                    ? StoredUser
                    : null);
        }

        public Task AddAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            StoredUser = user;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            UpdatedUser = user;
            StoredUser = user;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeRoleRepository : IRoleRepository
    {
        public Role? StoredRole { get; set; }

        public Task<Role?> GetByIdAsync(
            Guid roleId,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                StoredRole?.Id == roleId ? StoredRole : null);
        }

        public Task<Role?> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                StoredRole is not null &&
                string.Equals(
                    StoredRole.Name,
                    name,
                    StringComparison.OrdinalIgnoreCase)
                    ? StoredRole
                    : null);
        }

        public Task AddAsync(
            Role role,
            CancellationToken cancellationToken = default)
        {
            StoredRole = role;
            return Task.CompletedTask;
        }
    }
}
