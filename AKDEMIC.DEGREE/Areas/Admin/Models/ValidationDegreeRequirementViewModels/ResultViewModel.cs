using AKDEMIC.ENTITIES.Models.Degree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.DEGREE.Areas.Admin.Models.ValidationDegreeRequirementViewModels
{
    public class ResultViewModel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public Guid StudentId { get; set; }
        public int GradeType { get; set; }
        public IEnumerable<DegreeRequirement> DegreeRequirements { get; set; }
        public string FullName { get; set; }
        public string ProfessionalSchool { get; set; }
        public string Approvedcredits { get; set; }
    }
}
