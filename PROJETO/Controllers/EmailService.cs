using Microsoft.Extensions.Options;
using PROJETO.Models;
using System.Net;
using System.Net.Mail;

namespace PROJETO.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task EnviarEmailAsync(string nome, string emailUsuario, string mensagem)
        {
            var fromAddress = new MailAddress(_settings.Email, "Contato do Site");
            var toAddress = new MailAddress(_settings.Email); // você mesmo recebe

            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(_settings.Email, _settings.Senha)
            };

            var body = $@"
                <h2>Novo comentário recebido!</h2>
                <p><b>Nome:</b> {nome}</p>
                <p><b>E-mail do usuário:</b> {emailUsuario}</p>
                <p><b>Mensagem:</b><br>{mensagem}</p>
            ";

            var msg = new MailMessage(fromAddress, toAddress)
            {
                Subject = "Novo comentário do site",
                Body = body,
                IsBodyHtml = true
            };

            await smtp.SendMailAsync(msg);
        }
    }
}