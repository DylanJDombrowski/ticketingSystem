using System.ComponentModel.DataAnnotations;

namespace ClientTicketing.Core.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }
        
        public int TicketId { get; set; }
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int DurationMinutes { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public Ticket Ticket { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}