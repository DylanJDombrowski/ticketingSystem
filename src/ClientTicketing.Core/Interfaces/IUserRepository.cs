using ClientTicketing.Core.Models;

namespace ClientTicketing.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<User>> GetByTenantIdAsync(int tenantId);
        Task<int> GetCountByTenantIdAsync(int tenantId);
        Task<bool> ExistsAsync(int id);
    }
}