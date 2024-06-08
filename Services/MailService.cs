using DependencyInjectionAutomatic.Service;
using PRN221_Assignment.Services.Interface;
using System.Net.Mail;
using System.Net;
using Lombok.NET;
using PRN221_Assignment.Constant;

namespace PRN221_Assignment.Services
{
    [Service]
    [RequiredArgsConstructor]
    public partial class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;
        public bool SendMail(string to, string subject, string body)
        {
            MailMessage message = new MailMessage()
            {
                From = new MailAddress(ConstantMail.Sender, ConstantMail.SenderName),
                Subject = subject,
                Body = body,
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = true,
            };

            message.To.Add(new MailAddress(to));
            message.ReplyToList.Add(new MailAddress(ConstantMail.Sender));
            message.Sender = new MailAddress(ConstantMail.Sender);

            // Tạo SmtpClient kết nối đến smtp.gmail.com
            using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587;
                client.Credentials = new NetworkCredential(ConstantMail.Sender, ConstantMail.PasswordMail);
                client.EnableSsl = true;
                try
                {
                    client.SendMailAsync(message).Wait();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return false;
                }
            }

        }
    }
}
