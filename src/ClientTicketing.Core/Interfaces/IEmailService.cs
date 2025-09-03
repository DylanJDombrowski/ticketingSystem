namespace ClientTicketing.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendTicketNotificationAsync(int ticketId, string eventType);
        Task SendWelcomeEmailAsync(string email, string firstName, string tenantName);
        Task SendInvitationEmailAsync(string email, string inviteLink, string tenantName);
    }
}