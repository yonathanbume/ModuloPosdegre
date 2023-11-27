using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleLicenseAuthorization
{
    public class TeacherLicenseTemplate
    {
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string FullName { get; set; }

        public string Career { get; set; }
        public string PhoneNumber { get; set; }
        public string Dni { get; set; }
        public string Address { get; set; }


        public List<LicenseRecordTemplate> Licenses { get; set; }
    }

    public class LicenseRecordTemplate
    {
        public string Reason { get; set; }
        public string Observation { get; set; }
        public string ResolutionNumber { get; set; }
        public string Vigency { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
    }
}
