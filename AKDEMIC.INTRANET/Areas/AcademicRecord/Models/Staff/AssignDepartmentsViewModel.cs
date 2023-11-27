using System;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Models.Staff
{
    public class AssignDepartmentsViewModel
    {
        public string UserId { get; set; }
        public Guid[] AcademicDepartments { get; set; }
    }
}
