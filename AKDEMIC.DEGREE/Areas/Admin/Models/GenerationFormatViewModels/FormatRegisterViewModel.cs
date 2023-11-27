using System.Collections.Generic;

namespace AKDEMIC.DEGREE.Areas.Admin.Models.GenerationFormatViewModels
{
    public class FormatRegisterViewModel
    {
        public string RegistryNumber { get; set; }
        public string Book { get; set; }
        public string Folio { get; set; }
        public string GradeType { get; set; }
        public string Career { get; set; }
        public string FullName { get; set; }
        public string Faculty { get; set; }
        public string University { get; set; }
        public string Resolution { get; set; }
        public string DiplomaNum { get; set; }
        public string FacultyCouncilDate { get; set; }
        public string UniversityCouncilType { get; set; }
        public string UniversityCouncilDate { get; set; }
        public string GeneralSecretary { get; set; }
        public string DNI { get; set; }
        public string Image { get; set; }
    }

    public class ListFormatRegisterViewModel
    {
        public List<FormatRegisterViewModel> LstFormatRegister { get; set; }
    }
}
