using KerykeionCmsCore.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    /// <summary>
    /// Provides API's to send and receive emails.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly SendSmtpEmailOptions _options;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public EmailService(IOptions<SendSmtpEmailOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Call this function if you want to send an smtp email to one single user. Usefull in cases of email confirmation and password recovery.
        /// </summary>
        /// <param name="username">The username of the receiver.</param>
        /// <param name="email">The email address of the receiver</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="message">The message of the email</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionCms.Services.SendEmailResult of the operation.
        /// </returns>
        public async Task<SendEmailResult> SendEmailAsync(string username, string email, string subject, string message)
        {
            if (!_options.IsConfigured)
            {
                return SendEmailResult.IsNotConfigured;
            }
            try
            {
                var msgForUser = new MimeMessage();

                msgForUser.From.Add(new MailboxAddress(_options.WebsiteName, _options.WebsiteEmailAddress));
                msgForUser.To.Add(new MailboxAddress($"{username}", email));
                msgForUser.Subject = subject;
                msgForUser.Body = new TextPart(TextFormat.Html)
                {
                    Text = message
                };

                using var client = new SmtpClient();
                try
                {
                    await client.ConnectAsync(_options.SmtpHostName, _options.SmtpHostPort, _options.SmtpUseSsl);
                }
                catch (Exception ex)
                {
                    return new SendEmailResult(false, ex.Message);
                }
                await client.AuthenticateAsync(_options.AuthenticatedEmailAddress, _options.AuthenticatedEmailAddressPassword);
                try
                {
                    await client.SendAsync(msgForUser);
                }
                catch (SmtpCommandException ex)
                {
                    return new SendEmailResult(false, ex.Message);
                }
                await client.DisconnectAsync(true);
                return new SendEmailResult(true);
            }
            catch (Exception ex)
            {
                return new SendEmailResult(false, ex.Message);
            }
        }
    }
}
