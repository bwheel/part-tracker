using Microsoft.EntityFrameworkCore;
using PartTracker.Shared.Models;

namespace PartTracker.Server.Data
{
    public class PartTrackerContext : DbContext
    {

        private readonly IConfiguration m_configuration;

        public PartTrackerContext(IConfiguration configuration)
        {
            m_configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder) => builder.UseSqlite(m_configuration.GetConnectionString("PartsCabinet"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Category>()
                .HasIndex(c => c.Name)
                    .IsUnique();
            modelBuilder
                    .Entity<Project>()
                    .HasIndex(c => c.Name)
                    .IsUnique();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Part> Parts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<PartCategory> PartCategories { get; set; }
        public DbSet<PartImage> PartImages { get; set; }
        public DbSet<PartAttachment> PartAttachments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectAttachment> ProjectAttachments { get; set; }
        public DbSet<ProjectCategory> ProjectCategories { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }
    }
}
