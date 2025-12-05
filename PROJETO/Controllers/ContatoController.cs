using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PROJETO.Models;
using System.Net.Mail;
using System.Net;

namespace GOTTA.Controllers
{
    public class ContatoController : Controller
    {
        private readonly EmailSettings _emailSettings;

        public ContatoController(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnviarMensagem(string Nome, string Email, string Mensagem)
        {
            try
            {
                var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Senha),
                    EnableSsl = true
                };

                string corpo =
                    $"📩 Nova mensagem recebida pelo site:\n\n" +
                    $"Nome: {Nome}\n" +
                    $"E-mail: {Email}\n\n" +
                    $"Mensagem:\n{Mensagem}";

                var mail = new MailMessage
                {
                    From = new MailAddress(_emailSettings.Email),
                    Subject = "Contato recebido pelo site GOTTA",
                    Body = corpo
                };

                // Onde você recebe o e-mail
                mail.To.Add(_emailSettings.Email);

                // Para facilitar a resposta ao usuário
                mail.ReplyToList.Add(new MailAddress(Email));

                smtp.Send(mail);

                TempData["Sucesso"] = "Sua mensagem foi enviada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao enviar a mensagem: " + ex.Message;
            }

            return RedirectToAction("Contato", "Home");
        }
    }
}