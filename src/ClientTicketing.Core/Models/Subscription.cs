using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientTicketing.Core.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        
        public int TenantId { get; set; }
        public SubscriptionPlan Plan { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal MonthlyPrice { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        
        [MaxLength(100)]
        public string? StripeSubscriptionId { get; set; }
        
        // Plan limits
        public int MaxUsers { get; set; }
        public int MaxTickets { get; set; }
        public bool HasTimeTracking { get; set; }
        public bool HasReporting { get; set; }
        public bool HasApiAccess { get; set; }
        
        public Tenant Tenant { get; set; } = null!;
    }
}