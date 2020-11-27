using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Net;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MailKit;
using vds.Data;
using vds.Models;

namespace vds.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailSender : IEmailSender
    {
        private readonly Services.App.ICommon _app;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        public string mm;
        public EmailSender(
                Services.App.ICommon app,
                ApplicationDbContext context,
                IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            _app = app;
        }

        void OnMessageSent(object sender, MessageSentEventArgs e)
        {
            mm = "The message was sent!";

        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {

                EmailSenderConfig emailsenderinfo = new EmailSenderConfig();
                emailsenderinfo = _app.GetEmailSenderConfig();
 
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(emailsenderinfo.SenderName, emailsenderinfo.Email));
                mimeMessage.To.Add(MailboxAddress.Parse(email));

                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };

                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.MessageSent += OnMessageSent;
                    smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await smtp.ConnectAsync(emailsenderinfo.MailServer, emailsenderinfo.MailPort, MailKit.Security.SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(emailsenderinfo.Email, emailsenderinfo.Password);
                    await smtp.SendAsync(mimeMessage).ConfigureAwait(false);
                    await smtp.DisconnectAsync(true).ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }

    }
}