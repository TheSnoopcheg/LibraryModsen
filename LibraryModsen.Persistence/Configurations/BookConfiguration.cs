using LibraryModsen.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryModsen.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasMany(b => b.Authors)
            .WithMany(a => a.Books);

        builder
            .HasMany(b => b.BookStates)
            .WithOne(p => p.Book)
            .HasForeignKey(b => b.BookId);
    }
}
