using static AKDEMIC.CORE.Structs.Select2Structs;

namespace AKDEMIC.CORE.Services
{
    public interface ISelect2Service
    {
        int GetCurrentPage();
        string GetQuery();
        string GetRequestType();
        string GetSearchTerm();
        RequestParameters GetRequestParameters();
    }
}
