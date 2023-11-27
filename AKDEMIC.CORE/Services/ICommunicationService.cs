using AKDEMIC.CORE.Structs;

namespace AKDEMIC.CORE.Services
{
    public interface ICommunicationService
    {
        int GetNotificationPage();
        int GetNotificationRecords();
        CommunicationStructs.NotificationRequestParameters GetNotificationRequestParameters();
        CommunicationStructs.NotificationRequestParameters GetNotificationRequestParameters(string userId);
    }
}
