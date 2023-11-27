using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Teacher
{
    public class TeacherInformationTemplate
    {
        public string AcademicDepartment { get; set; }
        public Guid? AcademicDepartmentId { get; set; } = null;

        public string Dedication { get; set; }
        public Guid? TeacherDedicationId { get; set; } = null;

        public string Condition { get; set; }
        public Guid? WorkerLaborConditionId { get; set; } = null;

        public string Category { get; set; }
        public Guid? WorkerLaborCategoryId { get; set; } = null;

        public int Total { get; set; }
    }
}
