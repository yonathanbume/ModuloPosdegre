using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EnrolledStudent
{
    public class ReportOriginTemplate
    {
        public string DepartmentName { get; set; }
        public List<ProvinceOriginTemplate> Provinces { get; set; }

    }
    public class ProvinceOriginTemplate
    {
        public string ProvinceName { get; set; }
        public List<DistrictOriginTemplate> Districts { get; set; }
    }
    public class DistrictOriginTemplate
    {
        public string DistrictName { get; set; }
        public int Total { get; set; }
        public List<StudentReportOriginTemplate> Students { get; set; }
    }
    public class StudentReportOriginTemplate
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Career { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
    }
}
