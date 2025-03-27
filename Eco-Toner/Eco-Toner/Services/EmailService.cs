using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Eco_Toner.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task EnviarCorreoAsync(string destinatario, string asunto, string mensaje)
        {
            var smtpconfig = _configuration.GetSection("SmtpSettings");
            string host = smtpconfig["Host"];
            int port = int.Parse(smtpconfig["Port"]);
            bool enableSSL = bool.Parse(smtpconfig["EnableSSL"]);
            string username = smtpconfig["Username"];
            string password = smtpconfig["Password"];

            using(var client = new SmtpClient(host, port))
            {
                client.Credentials = new NetworkCredential(username, password);
                client.EnableSsl = enableSSL;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(username),
                    Subject = asunto,
                    Body = mensaje,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(destinatario);
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
