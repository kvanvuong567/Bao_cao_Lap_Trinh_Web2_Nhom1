using API_MPICTURE.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace API_MPICTURE.Data 
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image_Tag>()
                .HasOne(it => it.Image)
                .WithMany(i => i.Image_Tags)
                .HasForeignKey(it => it.ImageId);

            modelBuilder.Entity<Image_Tag>()
                .HasOne(it => it.Tag)
                .WithMany(t => t.Image_Tags)
                .HasForeignKey(it => it.TagId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Image_Tag> Image_Tags { get; set; }

        public DbSet<UpDownImage> UpDownImages { get; set; }
    }
}
