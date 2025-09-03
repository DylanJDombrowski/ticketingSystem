using ClientTicketing.Core.Models;

namespace ClientTicketing.Core.Interfaces
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(int id);
        Task<Client> CreateAsync(Client client);
        Task<Client> UpdateAsync(Client client);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Client>> GetByTenantIdAsync(int tenantId);
        Task<int> GetCountByTenantIdAsync(int tenantId);
    }
}