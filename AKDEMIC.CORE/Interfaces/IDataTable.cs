using System.Collections.Generic;

namespace AKDEMIC.CORE.Interfaces
{
    public interface IDataTable
    {
        int GetDataTableCurrentNumber();
        object GetDataTablePaginationObject<T>(int filterRecords, List<T> pagedList);
        int GetDataTableRecordsPerPage();
        string GetDataTableSortField();
        string GetDataTableSortOrder();
    }
}
