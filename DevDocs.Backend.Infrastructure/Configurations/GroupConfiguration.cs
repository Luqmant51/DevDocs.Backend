using DevDocs.Backend.Domain.Groups;
using DevDocs.Backend.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevDocs.Backend.Infrastructure.Configurations;

internal sealed class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("groups");

        builder.HasKey(group => group.Id);

        builder.Property(group => group.GroupName)
            .HasMaxLength(200)
            .HasConversion(groupName => groupName.Value, value => new Name(value));
        builder.Property(group => group.WorkspaceId)
            .HasMaxLength(200);
        builder.HasMany(x => x.Users)
            .WithMany()
            .UsingEntity(typeof(UserGroup));
    }
}