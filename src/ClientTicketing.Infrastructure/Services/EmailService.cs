using ClientTicketing.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClientTicketing.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // For MVP, we'll just log emails
            // Later, integrate with SendGrid, AWS SES, etc.
            _logger.LogInformation($"Sending email to {to}: {subject}");
            _logger.LogDebug($"Email body: {body}");
            
            // Simulate async email sending
            await Task.Delay(100);
        }

        public async Task SendTicketNotificationAsync(int ticketId, string eventType)
        {
            _logger.LogInformation($"Sending ticket notification for ticket {ticketId}, event: {eventType}");
            
            // In a real implementation, you'd:
            // 1. Get ticket details from repository
            // 2. Determine who to notify based on event type
            // 3. Send appropriate email template
            
            await Task.CompletedTask;
        }

        public async Task SendWelcomeEmailAsync(string email, string firstName, string tenantName)
        {
            var subject = $"Welcome to {tenantName}!";
            var body = $"Hello {firstName},\n\nWelcome to your new ticketing system!\n\nBest regards,\nThe Team";
            
            await SendEmailAsync(email, subject, body);
        }

        public async Task SendInvitationEmailAsync(string email, string inviteLink, string tenantName)
        {
            var subject = $"You've been invited to join {tenantName}";
            var body = $"You've been invited to join {tenantName}.\n\nClick here to accept: {inviteLink}";
            
            await SendEmailAsync(email, subject, body);
        }
    }
}