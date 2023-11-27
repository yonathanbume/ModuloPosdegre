using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class EnrolledByGroupsDataTemplate
    {
        public List<CourseTermsTemplate> StudentSectionByGroup { get; set; }
        public List<SectionCode> Codes { get; set; }
        public CurriculumTemplate Curriculum { get; set; }
    }
    public class CurriculumTemplate
    {
        public int Year { get; set; }
        public string Code { get; set; }
        public string TextCurriculum { get; set; }
    }
    public class SectionDataTemplate
    {
        public string Code { get; set; }
        public int students { get; set; }
    }
    public class CourseTermsTemplate
    {
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public byte AcademicYear { get; set; }

        public List<SectionDataTemplate> Sections { get; set; }
        public string Esp { get;set; }
        public string AcademicProgram { get;set; }
    }
    public class EnrolledByModalityDataTemplate
    {
        public List<CourseTermsTemplate> StudentSectionByGroup { get; set; }
        public List<SectionCode> Codes { get; set; }
    }
    public class CourseModalityTemplate
    {
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string AcademicYear => (intAcademicYear == 0) ? "-" : ConstantHelpers.ACADEMIC_YEAR.TEXT[intAcademicYear];
        public int intAcademicYear { get; set; }
        public string GroupName { get; set; }

        public Guid CourseId { get; set; }
        public int Regular { get; set; }
        public int Observed { get; set; }
        public int Reserved { get; set; }
        public int Third { get; set; }
        public int Fourth { get; set; }
        public int Fifth { get; set; }
        public int Sixth { get; set; }
        public int Seventh { get; set; }
        public int Eighth { get; set; }
        public int Ninth { get; set; }
        public int Tenth { get; set; }
        public int Eleventh { get; set; }
        public int Twelfth { get; set; }
        public int Thirteenth { get; set; }
        public int Fourteenth { get; set; }
        public int Fifteenth { get; set; }
        public int Dir { get; set; }
        public string Esp { get; set; }
        public string GroupingName => "NIVEL: -SEMESTRE: " + AcademicYear + " -GRUPO: " + GroupName;
        public int Total => Regular + Observed + Reserved + Third + Fourth + Fifth + Sixth + Seventh + Eighth + Ninth + Tenth + Eleventh + Twelfth + Thirteenth + Fourteenth + Fifteenth;
    }
}
