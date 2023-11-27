using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Student.Models.GradeViewModels
{
    public class CourseTermSyllabusViewModel
    {
        public Guid CourseId { get; set; }
        public Guid TermId { get; set; }
        public Guid CurriculumId { get; set; }
        public Guid CourseComponentId { get; set; }
        public Guid CourseSyllabusId { get; set; }
        public bool CanEdit { get; set; }
        public bool CanValidate { get; set; }
        public bool IsCoordinator { get; set; }
        public bool EnabledSyllabusValidation { get; set; }
        public SyllabusTeacherRequest SyllabusTeacherRequest { get; set; }

        //1
        public CourseTermSyllabusGeneralInformationViewModel GeneralInformation { get; set; }

        //2
        public string Summary { get; set; }

        //3
        public string GraduateProfile { get; set; }

        //4
        public string Competences { get; set; }

        //5
        public string LearningAchievement { get; set; }

        //6 
        public List<CourseUnitV2ViewModel> CourseUnits { get; set; }
        public List<CourseSyllabusWeekViewModel> CourseSyllabusWeek { get; set; }

        //7
        public MethodologicalStrategiesViewModel MethodologicalStrategies { get; set; }

        //8
        public string DidacticMaterials { get; set; }

        //9
        public LearningProductViewModel LearningProduct { get; set; }

        //10
        public string Raiting { get; set; }

        //11
        public BibliographicReferencesViewModel BibliographicReferences { get; set; }
    }

    public class SyllabusTeacherRequest
    {
        public Guid Id { get; set; }
        public byte Status { get; set; }
        public string PresentationDate { get; set; }
    }

    #region 1
    public class CourseTermSyllabusGeneralInformationViewModel
    {
        //Academic Information
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string PracticalHours { get; set; }
        public string SeminarHours { get; set; }
        public string TheoreticalHours { get; set; }
        public string VirtualHours { get; set; }
        public string TotalHours { get; set; }
        public string Prerequisites { get; set; }
        public string Credits { get; set; }
        public string Area { get; set; }
        public Guid? AreaId { get; set; }
        public byte Cycle { get; set; }
        public string Features { get; set; }
        public string TermName { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public DateTime TermClassStart { get; set; }
        public DateTime TermClassEnd { get; set; }
        public int TermYear { get; set; }
        public int TotalWeeks { get; set; }

        //Teacher Information
        public List<CourseTermSyllabusTeacherViewModel> Teachers { get; set; }

        //Classroom Information
        public string LearningEnvironment { get; set; } //TempData
        public List<CourseTermSyllabusClassroomViewModel> Classrooms { get; set; }
    }

    public class CourseTermSyllabusTeacherViewModel
    {
        public string TeacherId { get; set; }
        public bool IsTemporalName { get; set; }
        public bool IsTemporalCondition { get; set; }
        public bool IsTemporalSpeciality { get; set; }
        public string Name { get; set; }
        public string TeacherCondition { get; set; }
        public string TeacherSpeciality { get; set; }

        public bool IsCoordinator { get; set; }
    }

    public class CourseTermSyllabusClassroomViewModel
    {
        public string Building { get; set; }
        public string Campus { get; set; }
        public string Classroom { get; set; }
        public string FullName { get; set; }
    }
    #endregion

    #region 6

    public class CourseUnitV2ViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte Number { get; set; }
        public string LearningAchievements { get; set; }
        public string DevelopmentTime { get; set; }
        public string VirtualHours { get; set; }
        public string GradeEntryDate { get; set; }
        public string PerformanceCriterion { get; set; }
        public string EssentialKnowledge { get; set; }
        public int AcademicProgressPercentage { get; set; }
        public string PerformanceEvidence { get; set; }
        public int Weighing { get; set; }
        public bool EquivalentUnits { get; set; }
        public string Techniques { get; set; }
        public string Tools { get; set; }
        public int WeekNumberStart { get; set; }
        public int WeekNumberEnd { get; set; }
    }

    public class CourseSyllabusWeekViewModel
    {
        public Guid Id { get; set; }
        public int Week { get; set; }
        public string PerformanceCriterion { get; set; }
        public string EssentialKnowledge { get; set; }
    }

    #endregion

    #region 7
    public class MethodologicalStrategiesViewModel
    {
        public string Teaching { get; set; }
        public string Learning { get; set; }
        public string Research { get; set; }
        public string SocialResponsability { get; set; }
        public string VirtualTeaching { get; set; }
    }

    #endregion

    #region 9
    public class LearningProductViewModel
    {
        public string PresentationDate { get; set; }
        public string Product { get; set; }
    }
    #endregion

    #region 11
    public class BibliographicReferencesViewModel
    {
        public string Basic { get; set; }
        public string Complementary { get; set; }
        public string Electronic { get; set; }
        public string IntellectualProduction { get; set; }
    }
    #endregion
}
