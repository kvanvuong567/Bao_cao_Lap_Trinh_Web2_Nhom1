using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API_MPICTURE.Data
{
    public class ImageAuthDbContext : IdentityDbContext
    {
        public ImageAuthDbContext(DbContextOptions<ImageAuthDbContext> options) :
       base(options)
        {
        }
        // tạo phan quyen user và Admin cho hethong
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var userRoleId = "004c7e80 - 7dfc - 44be - 8952 - 2c7130898655";
            var adminRoleId = "71e282d3-76ca-485e-b094-eff019287fa5";
            base.OnModelCreating(builder);
            var roles = new List<IdentityRole>
        {
        new IdentityRole
        {
            Id = userRoleId,
            ConcurrencyStamp = userRoleId,
            Name ="User",
            NormalizedName="User".ToUpper()
        },
        new IdentityRole
        {
            Id = adminRoleId,
            ConcurrencyStamp = adminRoleId,
            Name ="Admin",
            NormalizedName="Admin".ToUpper()
        }
        };
                   builder.Entity<IdentityRole>().HasData(roles);
               }
           }

}
