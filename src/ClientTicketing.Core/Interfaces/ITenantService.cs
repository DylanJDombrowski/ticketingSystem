using ClientTicketing.Core.Models;

namespace ClientTicketing.Core.Interfaces
{
    public interface ITenantService
    {
        int GetCurrentTenantId();
        Task<Tenant?> GetCurrentTenantAsync();
        Task<bool> IsValidTenantAsync(string subdomain);
        void SetTenant(int tenantId);
        Task<bool> IsWithinUsageLimitsAsync(int tenantId, string resourceType);
    }
}