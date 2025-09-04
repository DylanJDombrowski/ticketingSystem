using Microsoft.AspNetCore.Mvc;
using ClientTicketing.Core.Interfaces;
using ClientTicketing.Core.Models;

namespace ClientTicketing.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantsController(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants()
        {
            var tenants = await _tenantRepository.GetAllAsync();
            return Ok(tenants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tenant>> GetTenant(int id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null)
                return NotFound();

            return Ok(tenant);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Tenant>> RegisterTenant(RegisterTenantDto dto)
        {
            if (await _tenantRepository.SubdomainExistsAsync(dto.Subdomain))
                return BadRequest("Subdomain already taken");

            var tenant = new Tenant
            {
                Name = dto.CompanyName,
                Subdomain = dto.Subdomain,
                Plan = SubscriptionPlan.Free,
                CreatedAt = DateTime.UtcNow,
                MaxUsers = 5,
                MaxTickets = 100,
                IsActive = true
            };

            var createdTenant = await _tenantRepository.CreateAsync(tenant);
            return CreatedAtAction(nameof(GetTenant), new { id = createdTenant.Id }, createdTenant);
        }
    }

    public class RegisterTenantDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Subdomain { get; set; } = string.Empty;
        public string AdminEmail { get; set; } = string.Empty;
        public string AdminFirstName { get; set; } = string.Empty;
        public string AdminLastName { get; set; } = string.Empty;
    }
}