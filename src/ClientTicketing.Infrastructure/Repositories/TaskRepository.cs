using Microsoft.EntityFrameworkCore;
using ClientTicketing.Core.Interfaces;
using ClientTicketing.Core.Models;
using ClientTicketing.Infrastructure.Data;
using TaskStatus = ClientTicketing.Core.Models.TaskStatus;

namespace ClientTicketing.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.Tasks
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.Ticket)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.Ticket)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            task.CreatedAt = DateTime.UtcNow;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem> UpdateAsync(TaskItem task)
        {
            if (task.Status == TaskStatus.Completed && task.CompletedAt == null)
            {
                task.CompletedAt = DateTime.UtcNow;
            }
            
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskItem>> GetByStatusAsync(TaskStatus status)
        {
            return await _context.Tasks
                .Where(t => t.Status == status)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.Ticket)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByAssignedUserIdAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.AssignedToId == userId)
                .Include(t => t.CreatedBy)
                .Include(t => t.Ticket)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByTicketIdAsync(int ticketId)
        {
            return await _context.Tasks
                .Where(t => t.TicketId == ticketId)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .OrderBy(t => t.Status)
                .ThenByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetOverdueAsync()
        {
            return await _context.Tasks
                .Where(t => t.DueDate.HasValue && 
                           t.DueDate < DateTime.UtcNow && 
                           t.Status != TaskStatus.Completed && 
                           t.Status != TaskStatus.Cancelled)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.Ticket)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }
    }
}