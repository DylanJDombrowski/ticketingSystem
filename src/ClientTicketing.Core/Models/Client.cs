using System.ComponentModel.DataAnnotations;

namespace ClientTicketing.Core.Models
{
    public class Client
    {
        public int Id { get; set; }
        
        public int TenantId { get; set; } // Multi-tenant key
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [MaxLength(100)]
        public string? Company { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public Tenant Tenant { get; set; } = null!;
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}