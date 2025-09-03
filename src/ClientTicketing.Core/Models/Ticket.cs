using System.ComponentModel.DataAnnotations;

namespace ClientTicketing.Core.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        
        public int TenantId { get; set; } // Multi-tenant key
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        
        public int ClientId { get; set; }
        public int? AssignedToId { get; set; }
        public int CreatedById { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        
        public int EstimatedHours { get; set; }
        public int ActualHours { get; set; }
        
        // Navigation properties
        public Tenant Tenant { get; set; } = null!;
        public Client Client { get; set; } = null!;
        public User? AssignedTo { get; set; }
        public User CreatedBy { get; set; } = null!;
        public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
        public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    }
}