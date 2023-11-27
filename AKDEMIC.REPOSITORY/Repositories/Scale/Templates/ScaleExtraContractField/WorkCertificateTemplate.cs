using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleExtraContractField
{
    public class WorkCertificateTemplate
    {
        public string ImagePathLogo { get; set; }
        public string FullName { get; set; }
        public string LaborPosition { get; set; }
        public int Sex { get; set; }
        public string Area { get; set; } //DependencyName
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string Dni { get; set; }

        public string TeacherDedication { get; set; }
        public string AcademicDepartment { get; set; }
        public string LaborCategory { get; set; }
        public string LaborCondition { get; set; }
    }
}
