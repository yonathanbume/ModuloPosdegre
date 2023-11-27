using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.GradeRectificationViewModels
{
    public class GradeRectificationViewModel
    {
        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        public IEnumerable<Guid> StudentIds { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        public Guid SectionId { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        public Guid TermId { get; set; }
        public Guid? EvaluationId { get; set; }
        public string TeacherId { get; set; }
        public int Type { get; set; }
        public int State { get; set; }
        public IFormFile ReasonFile { get; set; }
    }
}
