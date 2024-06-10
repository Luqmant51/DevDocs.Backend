namespace DevDocs.Backend.Domain.Groups;

public interface IGroupRepository
{
    Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(Group user);
}