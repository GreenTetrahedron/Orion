using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Orion.Logging.DataLayer
{
    public class LoggingDbContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }

        private IConfigurationRoot _configuration;

        private string _connectionString;

        public LoggingDbContext() : base()
        { }

        public LoggingDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = _configuration.GetConnectionString("Logging");

            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { }
    }
}
