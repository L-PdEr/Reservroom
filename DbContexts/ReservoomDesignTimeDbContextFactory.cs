using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Reservroom.DbContexts
{
    public class ReservoomDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ReservoomDbContext>
    {
        public ReservoomDbContext CreateDbContext(string[] args)
        {
            DbContextOptions options =  new DbContextOptionsBuilder()
                .UseSqlite("Data Source=Reservroom.db")
                .Options;

            return new ReservoomDbContext(options);
        }
    }
}
