using Microsoft.EntityFrameworkCore;
using Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental2._0.Tests
{
    public class DbContextFixture : IDisposable
    {
        public AppContextdb Context { get; private set; }

        public DbContextFixture()
        {
            var options = new DbContextOptionsBuilder<AppContextdb>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            Context = new AppContextdb(options);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
