using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ClientTicketing.Core.Interfaces;
using ClientTicketing.Core.Models;
using ClientTicketing.Infrastructure.Data;

namespace ClientTicketing.Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private int? _currentTenantId;

        public TenantService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public int GetCurrentTenantId()
        {
            if (_currentTenantId.HasValue)
                return _currentTenantId.Value;

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                // Try to get tenant from JWT claims first
                var tenantClaim = httpContext.User.FindFirst("tenant_id");
                if (tenantClaim != null && int.TryParse(tenantClaim.Value, out int tenantId))
                {
                    _currentTenantId = tenantId;
                    return tenantId;
                }

                // Fallback: extract from subdomain
                var host = httpContext.Request.Host.Host;
                var parts = host.Split('.');
                if (parts.Length > 1)
                {
                    var subdomain = parts[0];
                    // You could cache this lookup
                    var tenant = _context.Tenants.FirstOrDefault(t => t.Subdomain == subdomain && t.IsActive);
                    if (tenant != null)
                    {
                        _currentTenantId = tenant.Id;
                        return tenant.Id;
                    }
                }
            }

            // Default tenant for development/testing
            return 1;
        }

        public async Task<Tenant?> GetCurrentTenantAsync()
        {
            var tenantId = GetCurrentTenantId();
            return await _context.Tenants.FindAsync(tenantId);
        }

        public async Task<bool> IsValidTenantAsync(string subdomain)
        {
            return await _context.Tenants.AnyAsync(t => t.Subdomain == subdomain && t.IsActive);
        }

        public void SetTenant(int tenantId)
        {
            _currentTenantId = tenantId;
        }

        public async Task<bool> IsWithinUsageLimitsAsync(int tenantId, string resourceType)
        {
            var tenant = await _context.Tenants.FindAsync(tenantId);
            if (tenant == null) return false;

            return resourceType.ToLower() switch
            {
                "users" => tenant.CurrentUsers < tenant.MaxUsers,
                "tickets" => tenant.CurrentTickets < tenant.MaxTickets,
                _ => true
            };
        }
    }
}