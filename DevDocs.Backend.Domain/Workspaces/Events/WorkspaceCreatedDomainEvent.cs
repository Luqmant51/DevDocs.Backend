
using DevDocs.Backend.Domain.Abstractions;

namespace DevDocs.Backend.Domain.Workspaces.Events;

public sealed record WorkspaceCreatedDomainEvent(Guid WorkspaceId) : IDomainEvent;
