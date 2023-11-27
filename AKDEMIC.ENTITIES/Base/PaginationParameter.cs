namespace AKDEMIC.ENTITIES.Base
{
    public class PaginationParameter
    {
        public readonly string BaseOrder = "desc";
        public string SortOrder { get; set; }
        public string SortField { get; set; }
        public int CurrentNumber { get; set; }
        public int RecordsPerPage { get; set; }
        public string SearchValue { get; set; }
    }
}
