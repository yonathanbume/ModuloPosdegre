using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Degree.Templates
{
    public class DegreeRelationTemplate
    {
        public string Career { get; set; }
        public string FullName { get; set; }
        public int GradeType { get; set; }
        public string GradeAbreviation { get; set; }
    }
    public class ListDegreeRelationTemplate
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string MessageValidation { get; set; }
        public List<DegreeRelationTemplate> LstRelations { get; set; }
    }
}
