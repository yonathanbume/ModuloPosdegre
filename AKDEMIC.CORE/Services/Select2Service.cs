using AKDEMIC.CORE.Helpers;
using Microsoft.AspNetCore.Http;
using static AKDEMIC.CORE.Structs.Select2Structs;

namespace AKDEMIC.CORE.Services
{
    public class Select2Service : ISelect2Service
    {
        public IHttpContextAccessor _httpContextAccessor;

        public Select2Service(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetCurrentPage()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            int.TryParse(request.Query[ConstantHelpers.SELECT2.SERVER_SIDE.REQUEST_PARAMETERS.CURRENT_PAGE], out int result);
            return result;
        }

        public string GetQuery()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            return request.Query.ContainsKey(ConstantHelpers.SELECT2.SERVER_SIDE.REQUEST_PARAMETERS.QUERY) ?
                request.Query[ConstantHelpers.SELECT2.SERVER_SIDE.REQUEST_PARAMETERS.QUERY].ToString() :
                null;
        }

        public string GetRequestType()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            return request.Query.ContainsKey(ConstantHelpers.SELECT2.SERVER_SIDE.REQUEST_PARAMETERS.REQUEST_TYPE) ?
                request.Query[ConstantHelpers.SELECT2.SERVER_SIDE.REQUEST_PARAMETERS.REQUEST_TYPE].ToString() :
                null;
        }

        public string GetSearchTerm()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            return request.Query.ContainsKey(ConstantHelpers.SELECT2.SERVER_SIDE.REQUEST_PARAMETERS.SEARCH_TERM) ?
                request.Query[ConstantHelpers.SELECT2.SERVER_SIDE.REQUEST_PARAMETERS.SEARCH_TERM].ToString() :
                null;
        }

        public RequestParameters GetRequestParameters()
        {
            return new RequestParameters
            {
                CurrentPage = GetCurrentPage(),
                Query = GetQuery(),
                RequestType = GetRequestType(),
                SearchTerm = GetSearchTerm()
            };
        }
    }
}
