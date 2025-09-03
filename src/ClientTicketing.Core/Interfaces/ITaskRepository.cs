using ClientTicketing.Core.Models;

namespace ClientTicketing.Core.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(TaskItem task);
        Task<TaskItem> UpdateAsync(TaskItem task);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TaskItem>> GetByStatusAsync(Models.TaskStatus status);
        Task<IEnumerable<TaskItem>> GetByAssignedUserIdAsync(int userId);
        Task<IEnumerable<TaskItem>> GetByTicketIdAsync(int ticketId);
        Task<IEnumerable<TaskItem>> GetOverdueAsync();
    }
}