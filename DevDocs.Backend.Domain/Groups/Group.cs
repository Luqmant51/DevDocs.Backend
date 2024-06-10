using DevDocs.Backend.Domain.Abstractions;
using DevDocs.Backend.Domain.Shared;
using DevDocs.Backend.Domain.Users;
using System.Text.Json.Serialization;

namespace DevDocs.Backend.Domain.Groups;

public sealed class Group : Entity
{
    [JsonConstructor]
    private Group(Guid id, Name groupName, Guid workspaceId)
        : base(id)
    {
        GroupName = groupName;
        WorkspaceId = workspaceId;
    }

    private Group()
    {
    }

    public Name GroupName { get; private set; }

    public Guid WorkspaceId { get; private set; }

    public ICollection<User> Users { get; set; }

    public static Group Create(Name groupName, Guid workspaceId)
    {
        var group = new Group(Guid.NewGuid(), groupName, workspaceId);

        return group;
    }
}
