using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.GradeRecoveryViewModels
{
    public class GradeRecoveryExamViewModel
    {
        public Guid? ClassroomId { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public double Duration { get; set; }
        public Guid SectionId { get; set; }

        //Detalles
        public Guid GradeRecoveryExamId { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string Section { get; set; }
        public string Course { get; set; }
        public string Term { get; set; }
        public string Classroom { get; set; }
        public string EnrolledStudents { get; set; }
        public byte Status { get; set; }

        public string EvaluationTypeAssigned { get; set; }
    }
}
