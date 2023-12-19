using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orderly.Application.Entities;

namespace Orderly.Infrastructure.Data.EntityFramework.Config;

internal class TicketsConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(t => t.Id);

        builder
            .HasOne(t => t.Parent)
            .WithMany(t => t.Children)
            .HasForeignKey(t => t.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder
            .HasOne(t => t.User)
            .WithMany(u => u.Tickets)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
