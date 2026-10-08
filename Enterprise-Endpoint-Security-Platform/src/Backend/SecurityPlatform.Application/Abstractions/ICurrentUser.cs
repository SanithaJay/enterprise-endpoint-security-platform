namespace SecurityPlatform.Application.Abstractions;

public interface ICurrentUser
{
    Guid? UserId { get; }

    IReadOnlyCollection<string> Roles { get; }

    bool IsAuthenticated { get; }
}
