using AKDEMIC.CORE.Helpers;
using Microsoft.AspNetCore.Http;
using static AKDEMIC.CORE.Structs.CommunicationStructs;

namespace AKDEMIC.CORE.Services
{
    public class CommunicationService : ICommunicationService
    {
        public IHttpContextAccessor _httpContextAccessor;

        public CommunicationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetNotificationPage()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            int result = -1;

            if (request.Query.ContainsKey(ConstantHelpers.COMMUNICATION.NOTIFICATION.SERVER_SIDE.REQUEST_PARAMETERS.PAGE))
            {
                int.TryParse(request.Query[ConstantHelpers.COMMUNICATION.NOTIFICATION.SERVER_SIDE.REQUEST_PARAMETERS.PAGE], out result);
            }

            return result;
        }

        public int GetNotificationRecords()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            int result = -1;

            if (request.Query.ContainsKey(ConstantHelpers.COMMUNICATION.NOTIFICATION.SERVER_SIDE.REQUEST_PARAMETERS.RECORDS))
            {
                int.TryParse(request.Query[ConstantHelpers.COMMUNICATION.NOTIFICATION.SERVER_SIDE.REQUEST_PARAMETERS.RECORDS], out result);
            }

            return result;
        }

        public NotificationRequestParameters GetNotificationRequestParameters()
        {
            return new NotificationRequestParameters
            {
                Page = GetNotificationPage(),
                Records = GetNotificationRecords()
            };
        }

        public NotificationRequestParameters GetNotificationRequestParameters(string userId)
        {
            return new NotificationRequestParameters
            {
                Page = GetNotificationPage(),
                Records = GetNotificationRecords(),
                UserId = userId
            };
        }
    }
}
