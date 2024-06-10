using DevDocs.Backend.Domain.Abstractions;

namespace DevDocs.Backend.Domain.Shared;

public sealed class Role : Enumeration<Role>
{
    public static readonly Role WorkspaceAdmin = new(1, "WorkspaceAdmin");

    public static readonly Role WorkspaceContributor = new(2, "WorkspaceContributor");

    public static readonly Role WorkspaceReader = new(3, "WorkspaceReader");

    public Role(int id, string name)
        : base(id, name)
    {
    }
}
