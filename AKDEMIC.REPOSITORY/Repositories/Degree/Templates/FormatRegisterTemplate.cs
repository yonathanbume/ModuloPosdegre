using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Degree.Templates
{
    public class FormatRegisterTemplate
    {
        public string Logo { get; set; }
        public string RegistryNumber { get; set; }
        public string Book { get; set; }
        public string Folio { get; set; }
        public string GradeType { get; set; }
        public string Career { get; set; }
        public string FullName { get; set; }
        public string Faculty { get; set; }
        public string University { get; set; }
        public string Resolution { get; set; }
        public string ResolutionDateByUniversityCouncil { get; set; }
        public string DiplomaNum { get; set; }
        public string FacultyCouncilDate { get; set; }
        public string UniversityCouncilType { get; set; }
        public string UniversityCouncilDate { get; set; }
        public string AcademicDegreeDenomination { get; set; }
        public string DNI { get; set; }
        public string Image { get; set; }
        public int GradeTypeInt { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }

        public string UserPhoneNumber { get; set; }

        public string ManagingDirector { get; set; }
        public string ManagingDirectorFullName { get; set; }

        public string GeneralSecretary { get; set; }
        public string GeneralSecretaryFullName { get; set; }

        public string AcademicResponsible { get; set; }
        public string AcademicResponsibleFullName { get; set; }

        public string OriginDiplomatDate { get; set; }

    }
    public class ListFormatRegisterTemplate
    {
        public string Img { get; set; }
        public List<FormatRegisterTemplate> LstFormatRegisterTemplate { get; set; }
    }
}
