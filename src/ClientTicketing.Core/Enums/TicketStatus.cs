namespace ClientTicketing.Core.Models
{
    public enum TicketStatus
    {
        Open = 1,
        InProgress = 2,
        PendingReview = 3,
        Resolved = 4,
        Closed = 5,
        Cancelled = 6
    }
}