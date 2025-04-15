using Microsoft.EntityFrameworkCore;
using LibraryModsen.Persistence.Configurations;
using LibraryModsen.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LibraryModsen.Persistence;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookState> BookStates { get; set; }
    public DbSet<AppFile> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new AuthorConfiguration());
        modelBuilder.ApplyConfiguration(new BookStateConfiguration());

        List<Role> roles = new List<Role>()
        {
            new Role
            {
                Id = Guid.Parse("56b75cd9-837f-480d-8f2e-18913b046513"),
                Name = "Admin",
                NormalizedName= "ADMIN"
            },
            new Role
            {
                Id = Guid.Parse("4450a184-41b6-4918-b384-bfe12c35760b"),
                Name = "User",
                NormalizedName = "USER"
            }
        };
        modelBuilder.Entity<Role>().HasData(roles);

        base.OnModelCreating(modelBuilder);
    }
}
