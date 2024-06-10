using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevDocs.Backend.Domain.Documents;
using DevDocs.Backend.Domain.Shared;
using DevDocs.Backend.Domain.Workspaces;

namespace DevDocs.Backend.Infrastructure.Configurations;

internal sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("documents");

        builder.HasKey(doc => doc.DocumentId);
        builder.Property(doc => doc.DocumentName)
            .HasConversion(docName => docName.Value, value => new Name(value))
            .HasMaxLength(250)
            .IsRequired();

        builder
            .HasOne<Workspace>()
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}