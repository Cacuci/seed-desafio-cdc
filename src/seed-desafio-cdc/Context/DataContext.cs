using Microsoft.EntityFrameworkCore;

namespace seed_desafio_cdc.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Author>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasIndex(c => c.Email)
                      .IsUnique();

                entity.HasMany(c => c.Books)
                      .WithOne(c => c.Author)
                      .HasForeignKey(c => c.AuthorId)
                      .IsRequired();

            });

            builder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasIndex(c => c.Name)
                      .IsUnique();

                entity.HasMany(c => c.Books)
                      .WithOne(c => c.Category)
                      .HasForeignKey(c => c.CategoryId)
                      .IsRequired();
            });

            builder.Entity<Book>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasIndex(c => c.Title)
                      .IsUnique();

                entity.Property(c => c.Price).HasPrecision(10, 2);
            });
        }
    }
}