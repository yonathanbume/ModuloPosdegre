using System;

namespace AKDEMIC.REPOSITORY.Repositories.Degree.Templates
{
    public class RegistryPatternTemplate
    {
        public Guid? AcademicProgramId { get; set; }
        public Guid CareerId { get; set; }
        public Guid? GraduationTermId { get; set; }
        public Guid? ForeignUniversityOriginId { get; set; }
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string UniversityCode { get; set; }
        public string BussinesSocialReason { get; set; }
        public string RegistrationDate { get; set; }
        public string RegistrationEnd { get; set; }
        public string FacultyName { get; set; }
        public string CareerName { get; set; }
        public string PostGraduateSchool { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string StudentName { get; set; }
        public string UserName { get; set; }
        public string Sex { get; set; }
        public byte DocumentType { get; set; }
        public string DNI { get; set; }
        public string BachelorOrigin { get; set; }
        public string AcademicDegree { get; set; }
        public string AcademicDegreeDenomination { get; set; }
        public string Specialty { get; set; }
        public string ResearchWork { get; set; }
        public decimal Credits { get; set; }
        public string ResearchWorkURL { get; set; }
        public string DegreeProgram { get; set; }
        public string PedagogicalTitleOrigin { get; set; }
        public string ObtainingDegreeModality { get; set; }
        public string StudyModality { get; set; }
        public string GradeAbbreviation { get; set; }
        public string OriginDegreeCountry { get; set; }
        public string OriginDegreeUniversity { get; set; }
        public string GradDenomination { get; set; }
        public string ResolutionNumber { get; set; }
        public string ResolutionDateByUniversityCouncil { get; set; }
        public string OriginDiplomatDate { get; set; }
        public string DuplicateDiplomatDate { get; set; }
        public string DiplomatNumber { get; set; }
        public string EmissionTypeOfDiplomat { get; set; }
        public string BookCode { get; set; }
        public string FolioCode { get; set; }
        public string RegistryNumber { get; set; }
        public string ManagingDirector { get; set; }
        public string ManagingDirectorFullName { get; set; }
        public string GeneralSecretary { get; set; }
        public string GeneralSecretaryFullName { get; set; }
        public string AcademicResponsible { get; set; }
        public string AcademicResponsibleFullName { get; set; }
        public string OriginPreRequisiteDegreeCountry { get; set; }        
        public string OriginPreRequisiteDegreeDenomination { get; set; }
        public string OfficeNumber { get; set; }
        public string DateEnrollmentProgram { get; set; }
        public string StartDateEnrollmentProgram { get; set; }
        public string EndDateEnrollmentProgram { get; set; }
        public string AcademicProgram { get; set; }
        public string Relation { get; set; }
        public string SpecialtyMention { get; set; }
        public string UniversityCouncilType { get; set; }
        public string FacultyCouncilDate { get; set; }
        public string UniversityCouncilDate { get; set; }
        public string Initial { get; set; }
        public string Correlative { get; set; }
        public string Year { get; set; }
        public string BookTitle { get; set; }
        public string GraduationDate { get; set; }

        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Phone { get; set; }
    }

    public class RegistryPatternReportTemplate
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Dni { get; set; }
        public string CareerName { get; set; }
        public string GradeType { get; set; }
        public string RequestName { get; set; } //Trámite
        public string DiplomaDelivery { get; set; } //Estado del delivery
    }
}
