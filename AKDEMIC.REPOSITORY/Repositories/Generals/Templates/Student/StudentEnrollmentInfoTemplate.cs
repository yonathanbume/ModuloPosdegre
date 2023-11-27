using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class StudentEnrollmentInfoTemplate
    {
        public Guid Id { get; set; }
        public Guid CareerId { get; set; }
        public Guid FacultyId { get; set; }
        public string UserId { get; set; }
        public Guid CurriculumId { get; set; }
        public int Status { get; set; }
        public int AcademicYear { get; set; }
        public Guid? EnrollmentFeeId { get; set; }
        public Guid AdmissionTypeId { get; set; }
        public DateTime AdmissionTermStartDate { get; set; }
        public bool IsExoneratedEnrollment { get; set; }
        public string AdmissionTypeAbbrev { get; set; }
        public byte Condition { get; set; }
        public byte Benefit { get; set; }
        public int DiscountPercentage { get; set; } = 0;
    }
}