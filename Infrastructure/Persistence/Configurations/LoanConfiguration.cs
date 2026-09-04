using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("loans");

        builder.HasKey(l => l.Id);

        builder.HasIndex(l => l.BookId)
            .HasDatabaseName("ix_loans_book_id_active")
            .IsUnique();

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

        builder.HasOne(l => l.Book)
            .WithMany()
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.User)
            .WithMany(u => u.Loans)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}