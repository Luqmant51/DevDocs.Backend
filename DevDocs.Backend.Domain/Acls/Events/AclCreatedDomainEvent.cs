
using DevDocs.Backend.Domain.Abstractions;

namespace DevDocs.Backend.Domain.ACLs.Events;

public sealed record AclCreatedDomainEvent(string WorkspaceName, string GroupName, string RoleName) : IDomainEvent;
