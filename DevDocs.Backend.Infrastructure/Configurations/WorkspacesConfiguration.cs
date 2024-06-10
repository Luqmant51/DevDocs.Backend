
using DevDocs.Backend.Domain.Shared;
using DevDocs.Backend.Domain.Users;
using DevDocs.Backend.Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevDocs.Backend.Infrastructure.Configurations;

internal sealed class WorkspacesConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.ToTable("workspaces");

        builder.HasKey(workspace => workspace.Id);

        builder.Property(workspace => workspace.WorkspaceName)
            .HasMaxLength(200)
            .HasConversion(workspaceName => workspaceName.Value, value => new Name(value));
        builder.Property(workspace => workspace.IsPublic)
            .HasMaxLength(200);
        builder.Property(workspace => workspace.CreatedBy)
            .HasMaxLength(200);
        builder.Property(workspace => workspace.CreatedOnUtc)
            .HasMaxLength(200);
    }
}