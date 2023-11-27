using System;

namespace AKDEMIC.DEGREE.Areas.Admin.Models.DiplomaViewModels
{
    public class DiplomaViewModel
    {

        public Guid Id { get; set; }
        //4
        public string University { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string PostGraduateSchool { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string StudentName { get; set; }
        public string AcademicDegreeDenomination { get; set; }
        public string OriginDiplomatDate { get; set; }
        public string Rector { get; set; }
        public string RectorFullName { get; set; }
        public string GeneralSecretary { get; set; }
        public string GeneralSecretaryFullName { get; set; }
        public string AcademicResponsible { get; set; }
        public string AcademicResponsibleFullName { get; set; }

        //5
        public string UniversityCode { get; set; }
        public byte DocumentType { get; set; } 
        public string DocumentNumber { get; set; }
        public string GradeAbbreviation { get; set; }
        public byte ObtainingDegreeModality { get; set; }
        public string StudyModality { get; set; } 
        public string OriginDegreeCountry { get; set; }
        public string OriginDegreeUniversity { get; set; }
        public string OriginGradeDenomination { get; set; }
        public string ResolutionNumber { get; set; }

        //6
        public string ResolutionDateByUniversityCouncil { get; set; }
        public string DiplomatNumber { get; set; }
        public string EmissionTypeOfDiplomat { get; set; }
        public string DuplicateDiplomatDate { get; set; } 
        public string BookCode { get; set; }
        public string FolioCode { get; set; }
        public string RegistryNumber { get; set; }
    }
}
