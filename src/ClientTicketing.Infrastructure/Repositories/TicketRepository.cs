using Microsoft.EntityFrameworkCore;
using ClientTicketing.Core.Interfaces;
using ClientTicketing.Core.Models;
using ClientTicketing.Infrastructure.Data;

namespace ClientTicketing.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets
                .Include(t => t.Client)
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetByClientIdAsync(int clientId)
        {
            return await _context.Tickets
                .Where(t => t.ClientId == clientId)
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.Client)
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .Include(t => t.TimeEntries)
                    .ThenInclude(te => te.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Ticket> CreateAsync(Ticket ticket)
        {
            ticket.CreatedAt = DateTime.UtcNow;
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket> UpdateAsync(Ticket ticket)
        {
            ticket.UpdatedAt = DateTime.UtcNow;
            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Ticket>> GetByStatusAsync(TicketStatus status)
        {
            return await _context.Tickets
                .Where(t => t.Status == status)
                .Include(t => t.Client)
                .Include(t => t.AssignedTo)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetByAssignedUserIdAsync(int userId)
        {
            return await _context.Tickets
                .Where(t => t.AssignedToId == userId)
                .Include(t => t.Client)
                .Include(t => t.CreatedBy)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetRecentAsync(int count = 10)
        {
            return await _context.Tickets
                .Include(t => t.Client)
                .Include(t => t.AssignedTo)
                .OrderByDescending(t => t.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetCountByTenantIdAsync(int tenantId)
        {
            return await _context.Tickets
                .CountAsync(t => t.TenantId == tenantId);
        }

        public async Task<IEnumerable<Ticket>> GetOverdueAsync()
        {
            return await _context.Tickets
                .Where(t => t.DueDate.HasValue && 
                           t.DueDate < DateTime.UtcNow && 
                           t.Status != TicketStatus.Closed && 
                           t.Status != TicketStatus.Resolved)
                .Include(t => t.Client)
                .Include(t => t.AssignedTo)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }
    }
}