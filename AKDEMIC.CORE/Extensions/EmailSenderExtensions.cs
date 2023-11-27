using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public static class EmailSenderExtensions
    {
        public static void SendEmailConfirmation(this IEmailSender emailSender, string to, string link)
        {
            emailSender.SendEmail(to, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
    }
}
