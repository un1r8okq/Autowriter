using Autowriter.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Autowriter.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<SourceMaterial>? SourceMaterials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SourceMaterial>().ToTable("source_material");
        }
    }
}
