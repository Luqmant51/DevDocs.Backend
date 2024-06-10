
using DevDocs.Backend.Domain.ACLs;
using DevDocs.Backend.Domain.Groups;
using DevDocs.Backend.Domain.Shared;
using DevDocs.Backend.Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevDocs.Backend.Infrastructure.Configurations;

internal sealed class AclConfiguration : IEntityTypeConfiguration<Acl>
{
    public void Configure(EntityTypeBuilder<Acl> builder)
    {
        builder.ToTable("acls");

        builder.HasKey(acl => new { acl.GroupId, acl.WorkspaceId, acl.RoleId });

        builder
            .HasOne<Workspace>()
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<Group>()
            .WithMany()
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}