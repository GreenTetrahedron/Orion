using Microsoft.EntityFrameworkCore;
using Orion.Server.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
