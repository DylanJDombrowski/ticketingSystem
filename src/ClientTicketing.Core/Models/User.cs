using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ClientTicketing.Core.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        public int TenantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public Tenant Tenant { get; set; } = null!;
        public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
        public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();
    }

    public class Role : IdentityRole<int>
    {
        public string? Description { get; set; }
    }
}