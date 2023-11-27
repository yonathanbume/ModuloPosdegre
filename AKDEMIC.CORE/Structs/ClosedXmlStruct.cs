using System.Collections.Generic;

namespace AKDEMIC.CORE.Structs
{
    public struct ClosedXmlStruct<T>
    {
        public string FileName { get; set; }
        public ClosedXmlSheetStruct<T> Sheet { get; set; }
    }

    public struct ClosedXmlSheetStruct<T>
    {
        public string Title { get; set; }
        public string[] ColumnHeaders { get; set; }
        public List<T> Data { get; set; }
    }
}