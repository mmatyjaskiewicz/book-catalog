using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ArchivedLoanConfiguration : IEntityTypeConfiguration<ArchivedLoan>
{
    public void Configure(EntityTypeBuilder<ArchivedLoan> builder)
    {
        builder.ToTable("archived_loans");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("id");

        builder.Property(l => l.BookId)
            .HasColumnName("book_id")
            .IsRequired();

        builder.Property(l => l.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(l => l.BorrowedAt)
            .HasColumnName("borrowed_at")
            .IsRequired();

        builder.Property(l => l.ReturnedAt)
            .HasColumnName("returned_at")
            .IsRequired();

        builder.HasOne(l => l.Book)
            .WithMany()
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}