using LibraryModsen.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryModsen.Persistence.Configurations;

public class BookStateConfiguration : IEntityTypeConfiguration<BookState>
{
    public void Configure(EntityTypeBuilder<BookState> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(b => b.Book)
            .WithMany(p => p.BookStates);

        builder
            .HasOne(b => b.Holder)
            .WithMany(p => p.TakenBooks);
    }
}
