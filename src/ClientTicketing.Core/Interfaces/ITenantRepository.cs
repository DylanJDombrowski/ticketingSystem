using ClientTicketing.Core.Models;

namespace ClientTicketing.Core.Interfaces
{
    public interface ITenantRepository
    {
        Task<IEnumerable<Tenant>> GetAllAsync();
        Task<Tenant?> GetByIdAsync(int id);
        Task<Tenant?> GetBySubdomainAsync(string subdomain);
        Task<Tenant> CreateAsync(Tenant tenant);
        Task<Tenant> UpdateAsync(Tenant tenant);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> SubdomainExistsAsync(string subdomain);
    }
}