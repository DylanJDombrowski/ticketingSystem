using System.ComponentModel.DataAnnotations;

namespace ClientTicketing.Core.Models
{
    public class Tenant
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Subdomain { get; set; } = string.Empty; // e.g., "acme" for acme.yourapp.com
        
        [MaxLength(100)]
        public string? CustomDomain { get; set; } // e.g., "tickets.acme.com"
        
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public SubscriptionPlan Plan { get; set; }
        public DateTime? SubscriptionExpiresAt { get; set; }
        
        // Usage tracking
        public int MaxUsers { get; set; } = 5;
        public int MaxTickets { get; set; } = 100;
        public int CurrentUsers { get; set; }
        public int CurrentTickets { get; set; }
        
        // Navigation properties
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}