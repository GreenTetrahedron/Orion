using Microsoft.EntityFrameworkCore;
using Orion.Server.DataLayer;

namespace Orion.Server.Tests.Mocks.DataLayer
{
    public class MockOrionDbContext : OrionDbContext
    {
        public MockOrionDbContext()
        {
        }

        public MockOrionDbContext(DbContextOptions options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("MockOrionDatabase");
        }
    }
}
