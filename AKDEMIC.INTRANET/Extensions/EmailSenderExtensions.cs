using AKDEMIC.CORE.Services;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Services
{
    public static class EmailSenderExtensions
    {
        public static void SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            emailSender.SendEmail(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
    }
}
