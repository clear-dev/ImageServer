using Microsoft.EntityFrameworkCore;

namespace ImageAPI.Models
{
    public class ImageContext : DbContext
    {
        public ImageContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
    }
}
