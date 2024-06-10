using DevDocs.Backend.Domain.Abstractions;
using System.Text.Json.Serialization;

namespace DevDocs.Backend.Domain.ACLs;

public class Acl : Entity
{
    private Acl(Guid id, Guid workspaceId, Guid groupId, int roleId)
        : base(id)
    {
        WorkspaceId = workspaceId;
        GroupId = groupId;
        RoleId = roleId;
    }

    private Acl()
    {
    }

    public Guid WorkspaceId { get; private set; }

    public int RoleId { get; private set; }

    public Guid GroupId { get; private set; }

    //public static Acl Create(Workspace workspace, Group group, Role role)
    //{
        // var acl = new Acl(Guid.NewGuid(), workspace.Id, group.Id, role.Id);

        //acl.RaiseDomainEvent(
        //    new AclCreatedDomainEvent(
        //        workspace.WorkspaceName.Value,
        //        group.GroupName.Value,
        //        role.Name
        //        )
        //    );

        // return acl;
    //}
}
