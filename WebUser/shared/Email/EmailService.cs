namespace WebUser.shared.Email;

using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly SmtpClient _smtpClient;

    public EmailService()
    {
        _smtpClient = new SmtpClient("smtp.example.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("username", "password"),
            EnableSsl = true,
        };
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var mailMessage = new MailMessage("no-reply@example.com", email, subject, message);
        await _smtpClient.SendMailAsync(mailMessage);
    }
}
