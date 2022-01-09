using KindergartenManagementSystem.Core.Helpers;
using KindergartenManagementSystem.Core.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace KindergartenManagementSystem.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<Result> SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient();
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));

                mimeMessage.To.Add(new MailboxAddress(email, email));

                mimeMessage.Subject = subject;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = message
                };

                // For demo-purposes, accept all SSL certificates (in case the server supports START TLS)
                client.ServerCertificateValidationCallback = (_, _, _, _) => true;
                //client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                /*if (_env.IsDevelopment())
                    {*/
                // The third parameter is useSSL (true if the client should make an SSL-wrapped
                // connection to the server; otherwise, false).
                await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, true);
                /*}
                    else
                    {
                        await client.ConnectAsync(_emailSettings.MailServer);
                    }*/

                // Note: only needed if the SMTP server requires authentication
                await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

                await client.SendAsync(mimeMessage);

                await client.DisconnectAsync(true);

                return new Result
                {
                    Succeeded = true
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Succeeded = false
                };
            }
        }
    }
}
