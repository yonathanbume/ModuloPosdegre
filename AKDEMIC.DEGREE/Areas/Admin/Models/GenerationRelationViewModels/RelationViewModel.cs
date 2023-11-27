using System.Collections.Generic;

namespace AKDEMIC.DEGREE.Areas.Admin.Models.GenerationRelationViewModels
{
    public class RelationViewModel
    {
        public string Career { get; set; }
        public string FullName { get; set; }
        public string GradeType { get; set; }
    }
    public class ListRelationViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<RelationViewModel> LstRelations { get; set; }
    }
}
