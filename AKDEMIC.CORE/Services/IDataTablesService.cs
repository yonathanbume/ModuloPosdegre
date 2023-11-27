using System.Collections.Generic;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.CORE.Services
{
    public interface IDataTablesService
    {
        int GetDrawCounter();
        string GetOrderColumn();
        string GetOrderDirection();
        int GetPagingFirstRecord();
        int GetRecordsPerDraw();
        string GetSearchValue();
        SentParameters GetSentParameters();
        object GetPaginationObject<T>(int recordsFiltered, IEnumerable<T> data);
    }
}
