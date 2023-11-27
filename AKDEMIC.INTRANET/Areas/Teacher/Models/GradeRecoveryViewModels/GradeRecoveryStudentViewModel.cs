using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.GradeRecoveryViewModels
{
    public class GradeRecoveryStudentViewModel
    {
        public Guid GradeRecoveryExam { get; set; }
        public List<GradeRecoveryStudentsViewModel> Students { get; set; }
    }

    public class GradeRecoveryStudentsViewModel
    {
        public Guid StudentId { get; set; }
        public decimal? Value { get; set; }
        public bool IsAbsent { get; set; }
    }
}
