using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Overrides;
using AKDEMIC.ENTITIES.Models.Intranet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.DEGREE.Areas.Admin.Models.ValidationDegreeRequirementViewModels
{
    public class GradeReportViewModel
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public int GradeType { get; set; }
        public List<DegreeRequirementViewModel> DegreeRequirements { get; set; }    }

    public class GradeReportDetailViewModel
    {        
        public IEnumerable<GradeReportRequirement> GradeReportDegreeRequirements { get; set; }
    }

    public class DegreeRequirementViewModel
    {
        public Guid DegreeRequirementId { get; set; }
        [DataType(DataType.Upload)]
        [Extensions("pdf,docx,doc,dotx,dot", ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.FILE_EXTENSIONS)]
        public IFormFile DocumentFile { get; set; }
        public string Observation { get; set; }

    }
}
