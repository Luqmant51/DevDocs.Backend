using DevDocs.Backend.Domain.Abstractions;

namespace DevDocs.Backend.Domain.Groups;

public static class GroupErrors
{
    public static readonly Error NotFound = new(
        "Group.NotFound",
        "The group with the specified identifier was not found");
}
