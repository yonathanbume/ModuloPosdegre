using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Teacher
{
    public class TeacherLaborInformationTemplate
    {
        public string FullName { get; set; }
        public int MyProperty { get; set; }
        public string Specialty { get; set; }
        public string InstitutionType { get; set; }
        public string Institution { get; set; }
        public string ExpeditionDate { get; set; }
        public string Country { get; set; }
        public string Department { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public bool? JoinedBeforeLaw30220 { get; set; }
    }

}
