using AKDEMIC.CORE.Models;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public interface IEmailSender
    {
        void SendConfirmationPasswordEmail(string to, string message);
        void SendEmail(string to, string subject, string message, string projectName = null);
        void SendEmail(string to, string subject, string message, EmailCredentialModel model, string projectName = null);
        //Task SendEmailAsync(string email, string subject, string message);
        void SendEmailNotificationNewUser(string to, string systemUrl , string projectName, string userName , string password);
        void SendEmailPasswordRecovery(string projectName, string to, string url, string callbackUrl);
        void SendEmailToConfirmEmailCode(string projectName, string to, string url, string code);
        void SendEmailToConfirmEmail(string projectName, string to, string url, string callbackUrl, EmailCredentialModel model = null);
        void SendEmailRecoverPasswordQuibuk(string to, string subject, string message);
    }
}
