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
    optionsBuilder.UseNpgsql("Host=localhost;Database=ClientTicketingDb;Username=postgres;Password=password");
    
    return new AppDbContext(optionsBuilder.Options);
}
    }
}