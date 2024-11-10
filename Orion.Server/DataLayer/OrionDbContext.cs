using Microsoft.EntityFrameworkCore;
using Orion.Server.DirectCommuncations;
using Orion.Server.Messages;
using Orion.Server.Users;
using System.Configuration;

namespace Orion.Server.DataLayer
{
    public class OrionDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<DirectCommunication> DirectCommunications { get; set; }
        public DbSet<Message> Messages { get; set; }

        private string _connectionString;

        public OrionDbContext() : base()
        { }

        public OrionDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _connectionString = ConfigurationManager.ConnectionStrings["OrionDatabase"].ToString();
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Username)
                .IsUnique();
        }
    }
}
