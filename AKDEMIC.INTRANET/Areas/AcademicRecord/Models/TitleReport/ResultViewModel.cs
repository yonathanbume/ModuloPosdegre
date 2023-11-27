using AKDEMIC.ENTITIES.Models.Degree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Models.TitleReport
{
    public class ResultViewModel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public IEnumerable<DegreeRequirement> DegreeRequirements { get; set; }
    }
}
