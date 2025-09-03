using System.ComponentModel.DataAnnotations;

namespace ClientTicketing.Core.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        
        public int TenantId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        
        public int? TicketId { get; set; }  // Optional: link to ticket
        public int CreatedById { get; set; }
        public int? AssignedToId { get; set; }
        
        // Navigation properties
        public Tenant Tenant { get; set; } = null!;
        public Ticket? Ticket { get; set; }
        public User CreatedBy { get; set; } = null!;
        public User? AssignedTo { get; set; }
    }
}