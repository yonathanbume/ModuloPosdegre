using System;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class StudentFilterTemplate
    {
        public Guid Id { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Names { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string Dni { get; set; }
        public string Career { get; set; }
        public string GraduationTerm { get; set; }
        public int Status { get; set; }
        public string ResignationDateTime { get; set; }
    }
    public class StudentRankingByTermTemplate
    {
        public string Position { get; set; }
        public string AcademicYear { get; set; }
        public string Code { get; set; }
        public string Campus { get; set; }
        public string WeightedAverageGrade { get; set; }
        public string Credits { get; set; }
        public string MeritType { get; set; }
        public string Career { get; set; }
        public string Name { get; set; }
    }
    public class StudentRankingForCreditsTemplate
    {
        public int Position { get; set; }
        public string Name { get; set; }
        public string AcademicYear { get; set; }
        public string Code { get; set; }
        public string Career { get; set; }
        public string Campus { get; set; }
        public string WeightedAverageGrade { get; set; }
        public string Credits { get; set; }
        public decimal MaxCredits { get; set; }
    }
    public class GraduatedsTemplate
    {
        public int Position { get; set; }
        public string Code { get; set; }
        public string Dni { get; set; }
        public string Career { get; set; }
        public string Name { get; set; }
        public string AdmissionTerm { get; set; }
        public string GraduationTerm { get; set; }
        public string WeightedAverageGrade { get; set; }
        public int MeritOrder { get;  set; }
        public string MeritType { get;  set; }
    }
    public class NewStudentTemplate
    {
        public int Position { get; set; }
        public string Code { get;  set; }
        public string Dni { get;  set; }
        public string CareerCode { get;  set; }
        public string Name { get;  set; }
        public string CurriculumCode { get;  set; }
        public string FirstCampus { get;  set; }
        public string CurrentCampus { get;  set; }
        public string AdmissionTerm { get;  set; }
        public string LastTerm { get;  set; }
        public decimal? LastWeightedAverageGrade { get;  set; }
        public string GraduationTerm { get;  set; }
        public decimal? GraduationWeightedAverageGrade { get;  set; }
        public string Status { get;  set; }
        public string CurrentAcademicYear { get;  set; }
        public string MeritType { get;  set; }
    }
}
