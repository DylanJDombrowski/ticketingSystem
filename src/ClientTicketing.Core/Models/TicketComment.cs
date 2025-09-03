using System.ComponentModel.DataAnnotations;

namespace ClientTicketing.Core.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        
        public int TicketId { get; set; }
        public int UserId { get; set; }
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
        public bool IsInternal { get; set; } = false; // Internal notes vs client-visible
        
        // Navigation properties
        public Ticket Ticket { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}