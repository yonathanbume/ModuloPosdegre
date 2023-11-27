namespace AKDEMIC.CORE.Interfaces
{
    public interface IToastr
    {
        void ErrorToastMessage(string message, string title);
        void InfoToastMessage(string message, string title);
        void SuccessToastMessage(string message, string title);
        void WarningToastMessage(string message, string title);
    }
}
