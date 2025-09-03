using ClientTicketing.Core.Models;

namespace ClientTicketing.Core.Interfaces
{
    public interface ITimeEntryRepository
    {
        Task<IEnumerable<TimeEntry>> GetAllAsync();
        Task<TimeEntry?> GetByIdAsync(int id);
        Task<TimeEntry> CreateAsync(TimeEntry timeEntry);
        Task<TimeEntry> UpdateAsync(TimeEntry timeEntry);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TimeEntry>> GetByTicketIdAsync(int ticketId);
        Task<IEnumerable<TimeEntry>> GetByUserIdAsync(int userId);
        Task<IEnumerable<TimeEntry>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GetTotalMinutesByTicketIdAsync(int ticketId);
    }
}