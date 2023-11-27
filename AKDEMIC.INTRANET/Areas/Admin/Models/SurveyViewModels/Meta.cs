using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SurveyViewModels
{
    public class Meta
    {
      public int Page { get; set; }
      public int Pages { get; set; }
      public int PerPage { get; set; }
      public int Total { get; set; }
      public string Field { get; set; } 
      public IEnumerable<string> RowIds { get; set; }
      public bool SelectedAllRows { get; set; }
    }
}
