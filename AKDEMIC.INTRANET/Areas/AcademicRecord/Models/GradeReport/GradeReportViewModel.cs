using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Overrides;
using AKDEMIC.ENTITIES.Models.Intranet;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Models.GradeReport
{
    public class GradeReportViewModel
    {
        public Guid Id { get; set; }        
        public Guid StudentId { get; set; }
        public int Year { get; set; }
        public string Number { get; set; }
        public string Date { get; set; }
        public int YearsStudied { get; set; }
        public int SemesterStudied { get; set; }
        public int AdmissionYear { get; set; }
        public Guid GraduationTermId { get; set; }
        public decimal PromotionGrade { get; set; }
        public string Observation { get; set; }
        public Guid? ProcedureId { get; set; }
        public Guid? ConceptId { get; set; }

        public string ResearchWork { get; set; }
        public string ResearchWorkURL { get; set; }
        public decimal Credits { get; set; }
        public string BachelorOrigin { get; set; }
        public string PedagogicalTitleOrigin { get; set; }
        public string StudyModality { get; set; }
        public string OriginDegreeCountry { get; set; }
        public string GraduationDate { get; set; }
        public Guid RecordHistoryId { get; set; }
        public List<DegreeRequirementViewModel> DegreeRequirements { get; set; }

    }

    public class GradeReportDetailViewModel
    {
        public int Year { get; set; }
        public string Number { get; set; }
        public string Date { get; set; }
        public int YearsStudied { get; set; }
        public int SemesterStudied { get; set; }
        public Guid AdmissionTermId { get; set; }
        public Guid GraduationTermId { get; set; }
        public decimal PromotionGrade { get; set; }
        public string Observation { get; set; }
        public Guid? ProcedureId { get; set; }
        public Guid? ConceptId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string FacultyName { get; set; }
        public string CareerName { get; set; }
        public string CurricularSystem { get; set; }
        public string Curriculum { get; set; }
        public string AcademicProgram { get; set; }
        public bool IsIntegrated { get; set; }

        public string ResearchWork { get; set; }
        public string ResearchWorkURL { get; set; }
        public decimal Credits { get; set; }
        public string BachelorOrigin { get; set; }
        public string PedagogicalTitleOrigin { get; set; }
        public string StudyModality { get; set; }
        public string OriginDegreeCountry { get; set; }
        public string GraduationDate { get; set; }
        public IEnumerable<GradeReportRequirement> GradeReportDegreeRequirements { get; set; }

    }

    public class DegreeRequirementViewModel {
        public Guid DegreeRequirementId { get; set; }
        [DataType(DataType.Upload)]
        [Extensions("pdf,docx,doc,dotx,dot", ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.FILE_EXTENSIONS)]
        public IFormFile DocumentFile { get; set; }
        public string Observation { get; set; }

    }

}
