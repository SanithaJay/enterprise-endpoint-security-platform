using SecurityPlatform.Domain.Entities;

namespace SecurityPlatform.UnitTests.Domain;

public class RoleTests
{
    [Fact]
    public void Constructor_ShouldCreateRole()
    {
        var roleId = Guid.NewGuid();

        var role = new Role(
            roleId,
            "Security Admin");

        Assert.Equal(roleId, role.Id);
        Assert.Equal("Security Admin", role.Name);
        Assert.Empty(role.Permissions);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyRoleName()
    {
        Assert.Throws<ArgumentException>(() =>
            new Role(
                Guid.NewGuid(),
                ""));
    }

    [Fact]
    public void GrantPermission_ShouldAddPermission()
    {
        var role = new Role(
            Guid.NewGuid(),
            "Security Admin");

        var permission = new Permission(
            Guid.NewGuid(),
            "device.read",
            "View device information.");

        role.GrantPermission(permission);

        Assert.Single(role.Permissions);
        Assert.Contains(permission, role.Permissions);
    }

    [Fact]
    public void GrantPermission_ShouldNotAddDuplicatePermission()
    {
        var role = new Role(
            Guid.NewGuid(),
            "Security Admin");

        var permissionId = Guid.NewGuid();

        var firstPermission = new Permission(
            permissionId,
            "device.read",
            "View device information.");

        var secondPermission = new Permission(
            permissionId,
            "device.read",
            "View device information.");

        role.GrantPermission(firstPermission);
        role.GrantPermission(secondPermission);

        Assert.Single(role.Permissions);
    }

    [Fact]
    public void RevokePermission_ShouldRemovePermission()
    {
        var role = new Role(
            Guid.NewGuid(),
            "Security Admin");

        var permission = new Permission(
            Guid.NewGuid(),
            "device.read",
            "View device information.");

        role.GrantPermission(permission);
        role.RevokePermission(permission.Id);

        Assert.Empty(role.Permissions);
    }
}
