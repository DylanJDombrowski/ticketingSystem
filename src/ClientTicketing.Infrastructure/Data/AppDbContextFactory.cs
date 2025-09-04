using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ClientTicketing.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
public AppDbContext CreateDbContext(string[] args)
{
    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    
    // Try the simplest possible format
    optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ClientTicketingDb;Trusted_Connection=true;MultipleActiveResultSets=true");
    
    return new AppDbContext(optionsBuilder.Options);
}
    }
}