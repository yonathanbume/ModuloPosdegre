using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SurveyViewModels
{
    public class DatatableViewModel
    {
        public Meta Meta { get; set; }
        public IEnumerable<UserViewModel> Data { get; set; }
        
    }
}
