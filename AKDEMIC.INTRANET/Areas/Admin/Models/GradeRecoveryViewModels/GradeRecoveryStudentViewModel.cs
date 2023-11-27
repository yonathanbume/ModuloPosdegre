using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.GradeRecoveryViewModels
{
    public class GradeRecoveryStudentViewModel
    {
        public Guid GradeRecoveryExamId { get; set; }
        public List<Guid> Students { get; set; }
    }
}
