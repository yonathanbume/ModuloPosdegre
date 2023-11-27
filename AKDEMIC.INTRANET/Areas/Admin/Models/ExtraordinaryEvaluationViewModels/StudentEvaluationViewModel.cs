using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ExtraordinaryEvaluationViewModels
{
    public class StudentEvaluationViewModel
    {
        public Guid StudentId { get; set; }
        public Guid ExtraordinaryEvaluationId { get; set; }
    }
}
