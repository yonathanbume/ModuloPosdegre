using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using RestSharp;
using System;
//using System.Net;
//using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

//using MailKit.Net.Smtp;
//using MailKit.Security;

namespace AKDEMIC.CORE.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public void SendConfirmationPasswordEmail(string to, string message)
        {
            SendEmail(to, $"Cambia tu contraseña {GeneralHelpers.GetInstitutionAbbreviation()}", message);
        }

        public void SendEmail(string to, string subject, string message, string projectName = null)
        {
            var senderEmail = ConstantHelpers.Institution.SupportEmail[ConstantHelpers.GENERAL.Institution.Value];
            var senderEmailPassword = ConstantHelpers.Institution.SupportEmailPassword[ConstantHelpers.GENERAL.Institution.Value];

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(projectName ?? "Administrador", senderEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(senderEmail, senderEmailPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void SendEmail(string to, string subject, string message, EmailCredentialModel model, string projectName = null)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(projectName ?? "Administrador", model.Email));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            using var smtp = new SmtpClient();
            smtp.Connect(model.SmtpHost, model.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(model.Email, model.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void SendEmailNotificationNewUser(string to, string systemUrl, string projectName, string userName, string password)
        {
            var emailTemplate = new EmailHelpers().NewUserRegisterNotification(systemUrl, projectName, userName, password);
            SendEmail(to, $"Nuevo Usuario Creado en {projectName}", emailTemplate, projectName);
        }

        public void SendEmailPasswordRecovery(string projectName, string to, string url, string callbackUrl)
        {
            var emailhelper = new EmailHelpers();
            SendEmail(to, $"Recuperar contraseña en {projectName}", emailhelper.ForgotPassword(projectName, url, callbackUrl), projectName);
        }

        public void SendEmailToConfirmEmail(string projectName, string to, string url, string callbackUrl, EmailCredentialModel emailCredentials = null)
        {
            var emailhelper = new EmailHelpers();

            if (emailCredentials == null)
                SendEmail(to, $"Confirmar correo en {projectName}", emailhelper.ConfirmEmail(url, callbackUrl, projectName), projectName);
            else
                SendEmail(to, $"Confirmar correo en {projectName}", emailhelper.ConfirmEmail(url, callbackUrl, projectName), emailCredentials, projectName);
        }

        public void SendEmailToConfirmEmailCode(string projectName, string to, string url, string code)
        {
            var emailhelper = new EmailHelpers();
            SendEmail(to, $"Confirmar correo en {projectName}", emailhelper.EmailConfirmationCodeFormat(url, code), projectName);
        }

        //QUIBUK configured
        public void SendEmailRecoverPasswordQuibuk(string to, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Administrador Quibuk", "noreply@gmail.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("noreply@gmail.com", "Akdemic.2018");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
