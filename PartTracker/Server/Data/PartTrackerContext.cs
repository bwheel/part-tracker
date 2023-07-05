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

        protected override void OnConfiguring(DbContextOptionsBuilder builder) => builder.UseSqlite(m_configuration.GetConnectionString("PartsTracker"));
        
        public DbSet<Part> Parts { get; set; }
    }
}
