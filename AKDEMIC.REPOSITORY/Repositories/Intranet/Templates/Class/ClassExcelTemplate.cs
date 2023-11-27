using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Class
{
    public class ClassExcelTemplate
    {
        public string Schedule { get; set; }
        public string Career { get; set; }
        public string Course { get; set; } //Componente Curricular
        public byte AcademicYear { get; set; }
        public string AcademicDepartments { get; set; }
        public string SectionCode { get; set; }
        public int StudentsCount { get; set; }
        public string Teachers { get; set; }
        public string Subject { get; set; }
        public string Dictated { get; set; }
        public string VirtualClass { get; set; }
        public string Classroom { get; set; }
    }
}
