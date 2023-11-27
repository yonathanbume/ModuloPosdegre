using System.Collections.Generic;

namespace AKDEMIC.CORE.Structs
{
    public class DataTablesStructs
    {
        public struct SentParameters
        {
            public int DrawCounter { get; set; }
            public string OrderColumn { get; set; }
            public string OrderDirection { get; set; }
            public int PagingFirstRecord { get; set; }
            public int RecordsPerDraw { get; set; }
            public string SearchValue { get; set; }
        }

        public struct ReturnedData<T>
        {
            public IEnumerable<T> Data { get; set; }
            public int DrawCounter { get; set; }
            public string Error { get; set; }
            public int RecordsFiltered { get; set; }
            public int RecordsTotal { get; set; }
        }
    }
}
