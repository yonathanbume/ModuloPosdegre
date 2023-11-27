using AKDEMIC.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Templates.SuneduReport
{
    public class SuneduReportTemplate
    {
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string UserCountry { get; set; }
        public string Name { get; set; }
        public string Dni { get; set; }
        public string Document { get; set; }
        public string DocumentType { get; set; }
        public string Sex { get; set; }
        public string TermName { get; set; }
        public string BirthDate { get; set; }
        public string Depedencies { get; set; }
        public string CreatedDate { get; set; }
        public string LaborCategory { get; set; }
        public string LaborCondition { get; set; }
        public string LaborRegime { get; set; }
        public string CapPositionCode { get; set; }
        public string CapPositionName { get; set; }
        public StudiesTemplate ElementarySchoolStudies { get; set; } = null;
        public StudiesTemplate HighSchoolStudies { get; set; } = null;
        public StudiesTemplate TechnicalStudies { get; set; } = null;
        public StudiesTemplate BachelorDegrees { get; set; } = null;
        public StudiesTemplate ProfessionalSchools { get; set; } = null;
        public StudiesTemplate ProfessionalTitles { get; set; } = null;
        public StudiesTemplate MasterDegrees { get; set; } = null;
        public StudiesTemplate DoctoralDegrees { get; set; } = null;
        public StudiesTemplate SecondSpecialties { get; set; } = null;
        public StudiesTemplate Diplomates { get; set; } = null;
        public ContractTemplate ActualContract { get; set; }
        public string JoinedDateString { get; set; }
        public DateTime? JoinedDate{ get; set; }
        public decimal LectiveHours { get; set; }
        public decimal NoLectiveHours { get; set; }
        public string TeachPregrado { get; set; }
        public string TeachMaster { get; set; }
        public string TeachDoctor { get; set; }

        public bool DoesResearchTeacher { get; set; }
        public bool RenacytTeacher { get; set; }
        public bool ResearcherTeacher { get; set; }
    }

    public class TeacherSuneduReportTemplate: SuneduReportTemplate
    {
        public string TeacherDedication { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
    }

    public class AdministrativeSuneduReportTemplate : SuneduReportTemplate
    {
        //Add if needed
    }

    public class S3TeacherSuneduReportTemplate: SuneduReportTemplate
    {
        public List<string> CampusCodes { get; set; }
        public List<string> AcademicPrograms { get; set; }
        public decimal NotLectiveAcademicHours { get; set; }
        public decimal NotLectiveResearchHours { get; set; }
        public decimal NotLectiveOtherHours { get; set; }
        public string Observations { get; set; }
    }

    public class StudiesTemplate
    {
        public string Country { get; set; }
        public string Speciality { get; set; }
        public string InsitutionName { get; set; }
    }

    public class ContractTemplate
    {
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
    }
}
