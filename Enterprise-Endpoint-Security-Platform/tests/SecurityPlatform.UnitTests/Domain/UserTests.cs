using SecurityPlatform.Domain.Entities;
using SecurityPlatform.Domain.Enums;

namespace SecurityPlatform.UnitTests.Domain;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldCreateActiveUser()
    {
        var userId = Guid.NewGuid();

        var user = new User(
            userId,
            "sanitha",
            "sanitha@example.com");

        Assert.Equal(userId, user.Id);
        Assert.Equal("sanitha", user.Username);
        Assert.Equal("sanitha@example.com", user.Email);
        Assert.Equal(AccountStatus.Active, user.Status);
        Assert.Empty(user.Roles);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyUserId()
    {
        Assert.Throws<ArgumentException>(() =>
            new User(
                Guid.Empty,
                "sanitha",
                "sanitha@example.com"));
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyUsername()
    {
        Assert.Throws<ArgumentException>(() =>
            new User(
                Guid.NewGuid(),
                "",
                "sanitha@example.com"));
    }

    [Fact]
    public void AssignRole_ShouldAddRole()
    {
        var user = new User(
            Guid.NewGuid(),
            "sanitha",
            "sanitha@example.com");

        var role = new Role(
            Guid.NewGuid(),
            "Security Admin");

        user.AssignRole(role);

        Assert.Single(user.Roles);
        Assert.Contains(role, user.Roles);
    }

    [Fact]
    public void AssignRole_ShouldNotAddDuplicateRole()
    {
        var user = new User(
            Guid.NewGuid(),
            "sanitha",
            "sanitha@example.com");

        var roleId = Guid.NewGuid();

        var firstRole = new Role(roleId, "Security Admin");
        var secondRole = new Role(roleId, "Security Admin");

        user.AssignRole(firstRole);
        user.AssignRole(secondRole);

        Assert.Single(user.Roles);
    }

    [Fact]
    public void Disable_ShouldChangeStatusToDisabled()
    {
        var user = new User(
            Guid.NewGuid(),
            "sanitha",
            "sanitha@example.com");

        user.Disable();

        Assert.Equal(AccountStatus.Disabled, user.Status);
    }

    [Fact]
    public void Activate_ShouldChangeStatusToActive()
    {
        var user = new User(
            Guid.NewGuid(),
            "sanitha",
            "sanitha@example.com");

        user.Disable();
        user.Activate();

        Assert.Equal(AccountStatus.Active, user.Status);
    }

    [Fact]
    public void Lock_ShouldChangeStatusToLocked()
    {
        var user = new User(
            Guid.NewGuid(),
            "sanitha",
            "sanitha@example.com");

        user.Lock();

        Assert.Equal(AccountStatus.Locked, user.Status);
    }

    [Fact]
    public void RemoveRole_ShouldRemoveAssignedRole()
    {
        var user = new User(
            Guid.NewGuid(),
            "sanitha",
            "sanitha@example.com");

        var role = new Role(
            Guid.NewGuid(),
            "Security Admin");

        user.AssignRole(role);
        user.RemoveRole(role.Id);

        Assert.Empty(user.Roles);
    }
}
