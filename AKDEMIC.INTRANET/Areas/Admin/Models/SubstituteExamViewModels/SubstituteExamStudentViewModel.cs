using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SubstituteExamViewModels
{
    public class SubstituteExamStudentViewModel
    {
        public Guid StudentId { get; set; }
        public Guid SectionId { get; set; }
        public bool Checked { get; set; }
    }
}
