using Microsoft.EntityFrameworkCore;
using ClientTicketing.Core.Interfaces;
using ClientTicketing.Core.Models;
using ClientTicketing.Infrastructure.Data;

namespace ClientTicketing.Infrastructure.Repositories
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly AppDbContext _context;

        public TimeEntryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TimeEntry>> GetAllAsync()
        {
            return await _context.TimeEntries
                .Include(te => te.Ticket)
                .Include(te => te.User)
                .OrderByDescending(te => te.StartTime)
                .ToListAsync();
        }

        public async Task<TimeEntry?> GetByIdAsync(int id)
        {
            return await _context.TimeEntries
                .Include(te => te.Ticket)
                .Include(te => te.User)
                .FirstOrDefaultAsync(te => te.Id == id);
        }

        public async Task<TimeEntry> CreateAsync(TimeEntry timeEntry)
        {
            timeEntry.CreatedAt = DateTime.UtcNow;
            _context.TimeEntries.Add(timeEntry);
            await _context.SaveChangesAsync();
            return timeEntry;
        }

        public async Task<TimeEntry> UpdateAsync(TimeEntry timeEntry)
        {
            // Recalculate duration if end time is set
            if (timeEntry.EndTime.HasValue)
            {
                var duration = timeEntry.EndTime.Value - timeEntry.StartTime;
                timeEntry.DurationMinutes = (int)duration.TotalMinutes;
            }

            _context.Entry(timeEntry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return timeEntry;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var timeEntry = await _context.TimeEntries.FindAsync(id);
            if (timeEntry == null) return false;

            _context.TimeEntries.Remove(timeEntry);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TimeEntry>> GetByTicketIdAsync(int ticketId)
        {
            return await _context.TimeEntries
                .Where(te => te.TicketId == ticketId)
                .Include(te => te.User)
                .OrderBy(te => te.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<TimeEntry>> GetByUserIdAsync(int userId)
        {
            return await _context.TimeEntries
                .Where(te => te.UserId == userId)
                .Include(te => te.Ticket)
                .OrderByDescending(te => te.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<TimeEntry>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.TimeEntries
                .Where(te => te.StartTime >= startDate && te.StartTime <= endDate)
                .Include(te => te.Ticket)
                .Include(te => te.User)
                .OrderBy(te => te.StartTime)
                .ToListAsync();
        }

        public async Task<int> GetTotalMinutesByTicketIdAsync(int ticketId)
        {
            return await _context.TimeEntries
                .Where(te => te.TicketId == ticketId)
                .SumAsync(te => te.DurationMinutes);
        }
    }
}