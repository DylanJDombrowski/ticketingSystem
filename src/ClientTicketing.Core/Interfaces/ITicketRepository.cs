using ClientTicketing.Core.Models;

namespace ClientTicketing.Core.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<IEnumerable<Ticket>> GetByClientIdAsync(int clientId);
        Task<Ticket?> GetByIdAsync(int id);
        Task<Ticket> CreateAsync(Ticket ticket);
        Task<Ticket> UpdateAsync(Ticket ticket);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Ticket>> GetByStatusAsync(TicketStatus status);
        Task<IEnumerable<Ticket>> GetByAssignedUserIdAsync(int userId);
        Task<IEnumerable<Ticket>> GetRecentAsync(int count = 10);
        Task<int> GetCountByTenantIdAsync(int tenantId);
        Task<IEnumerable<Ticket>> GetOverdueAsync();
    }
}