using Microsoft.EntityFrameworkCore;
using ClientTicketing.Core.Interfaces;
using ClientTicketing.Core.Models;
using ClientTicketing.Infrastructure.Data;

namespace ClientTicketing.Infrastructure.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly AppDbContext _context;

        public TenantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return await _context.Tenants
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<Tenant?> GetByIdAsync(int id)
        {
            return await _context.Tenants
                .Include(t => t.Users)
                .Include(t => t.Clients)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tenant?> GetBySubdomainAsync(string subdomain)
        {
            return await _context.Tenants
                .FirstOrDefaultAsync(t => t.Subdomain == subdomain && t.IsActive);
        }

        public async Task<Tenant> CreateAsync(Tenant tenant)
        {
            tenant.CreatedAt = DateTime.UtcNow;
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();
            return tenant;
        }

        public async Task<Tenant> UpdateAsync(Tenant tenant)
        {
            _context.Entry(tenant).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return tenant;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null) return false;

            tenant.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Tenants.AnyAsync(t => t.Id == id && t.IsActive);
        }

        public async Task<bool> SubdomainExistsAsync(string subdomain)
        {
            return await _context.Tenants.AnyAsync(t => t.Subdomain == subdomain);
        }
    }
}