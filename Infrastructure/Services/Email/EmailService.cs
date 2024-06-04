using Application.Contracts.Email;
using Infrastructure.Services.Email.Body;
using Infrastructure.Services.Email.HTMLTemplates;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContext;

        private readonly string _displayName;
        private readonly string _from;
        private readonly bool _useSSL;
        private readonly bool _useStartTls;
        private readonly string _host;
        private readonly int _port;
        private readonly string _password;
        private readonly string _username;

        public EmailService(IConfiguration config,
                            IHttpContextAccessor httpContext)
        {
            _config = config;
            _httpContext = httpContext;

            _displayName = _config["MailSettings:DisplayName"] ?? throw new ArgumentNullException("MailSettings:DisplayName");
            _from = _config["MailSettings:From"] ?? throw new ArgumentNullException("MailSettings:From");
            _useSSL = bool.TryParse(_config["MailSettings:UseSSL"], out var useSSL) && useSSL;
            _useStartTls = bool.TryParse(_config["MailSettings:UseStartTls"], out var useStartTls) && useStartTls;
            _host = _config["MailSettings:Host"] ?? throw new ArgumentNullException("MailSettings:Host");
            _port = int.TryParse(_config["MailSettings:Port"], out var port) ? port : throw new ArgumentNullException("MailSettings:Port");
            _password = _config["MailSettings:Password"] ?? throw new ArgumentNullException("MailSettings:Password");
            _username = _config["MailSettings:UserName"] ?? throw new ArgumentNullException("MailSettings:UserName");
        }

        private async Task<bool> SendAsync(EmailData emailData, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(emailData);
            try
            {
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress(_displayName, _from));
                mail.Sender = new MailboxAddress(_displayName, _from);
                mail.To.Add(MailboxAddress.Parse(emailData.To));

                var body = new BodyBuilder
                {
                    HtmlBody = emailData.Body
                };
                mail.Subject = emailData.Subject;
                mail.Body = body.ToMessageBody();

                using var smtp = new SmtpClient();

                if (_useSSL)
                {
                    await smtp.ConnectAsync(_host, _port, SecureSocketOptions.SslOnConnect, cancellationToken);
                }
                else if (_useStartTls)
                {
                    await smtp.ConnectAsync(_host, _port, SecureSocketOptions.StartTls, cancellationToken);
                }
                else
                {
                    await smtp.ConnectAsync(_host, _port, SecureSocketOptions.None, cancellationToken);
                }

                await smtp.AuthenticateAsync(_username, _password, cancellationToken);
                await smtp.SendAsync(mail, cancellationToken);
                await smtp.DisconnectAsync(true, cancellationToken);

                return true;
            }
            catch (Exception)
            {
                // Log
                throw;
            }
        }

        public async Task SendConfirmationEmailAsync(string token, string email, CancellationToken cancellationToken)
        {
            var body = await BodyTemplates.VerifyEmailBody(GetUrl(), email, token);
            
            _ = await SendAsync(new EmailData { 
                To = email,
                Subject = "Confirm Your Email",
                Body = body
            }, cancellationToken);
        }

        private string GetUrl()
        {
            var url = _httpContext.HttpContext?.Request?.Host.ToString();
            url = "https://" + url;
            return url;
        }
    }
}
