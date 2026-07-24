using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Options;

using MimeKit;

using cmkapi.DTO;
using cmkapi.Services.Interfaces;

namespace cmkapi.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _setting;

    public EmailService(
        IOptions<EmailSettings> options)
    {
        _setting = options.Value;
    }

    public async Task SendAsync(
        string to,
        string subject,
        string html)
    {
        var email = new MimeMessage();

        email.From.Add(
            new MailboxAddress(
                _setting.DisplayName,
                _setting.From));

        email.To.Add(
            MailboxAddress.Parse(to));

        email.Subject = subject;

        email.Body = new BodyBuilder
        {
            HtmlBody = html
        }.ToMessageBody();

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _setting.Host,
            _setting.Port,
            SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            _setting.Username,
            _setting.Password);

        await smtp.SendAsync(email);

        await smtp.DisconnectAsync(true);
    }
}