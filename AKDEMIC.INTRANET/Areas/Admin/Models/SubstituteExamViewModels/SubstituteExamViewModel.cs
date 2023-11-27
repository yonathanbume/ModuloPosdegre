using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SubstituteExamViewModels
{
    public class SubstituteExamViewModel
    {
        public bool IsCheckAll { get; set; }
        public Guid SectionId { get; set; }
        public List<Guid> LstToAdd { get; set; } = new List<Guid>();
        public List<Guid> LstToAvoid { get; set; } = new List<Guid>();
    }
    public class NewSubstituteExamViewModel
    {
        public Guid CareerId { get; set; }
        public Guid SectionId { get; set; }
        public int CycleId { get; set; }
        public Guid CourseId { get; set; }
        public Guid ClassroomId { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public double Duration { get; set; }


    }
    public class SectionSubstituteExamViewModel
    {
        public Guid id { get; set; }
        public Guid courseTermId { get; set; }
        public string code { get; set; }
        public string career { get; set; }
        public string name { get; set; }
        public string academicYear { get; set; }
        public string modality { get; set; }
        public string teachers { get; set; }
        public string groupcycle { get; set; }
        public string group { get; set; }
    }
    public class SectionSubstituteExamPdfViewModel
    {
        public string Img { get; set; }
        public string Term { get; set; }
        public string Course { get; set; }
        public string Group { get; set; }
        public string Career { get; set; }
        public List<SubstituteExamPdfViewModel> Rows { get; set; }
    }
    public class SubstituteExamPdfViewModel
    {
        public string Code { get; set; }
        public bool Status { get; set; }
        public string Grade { get; set; }
        public string UserName { get; set; }
    }

    public class SectionsPdfViewModel
    {
        public string Img { get; set; }
        public string Course { get; set; }
        public string Cycle { get; set; }
        public string Curriculum { get; set; }
        public string Career { get; set; }
        public List<SectionDataPdfViewModel> Rows { get; set; }
    }
    public class SectionDataPdfViewModel
    {
        public Guid id { get; set; }
        public Guid courseTermId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string academicYear { get; set; }
        public string modality { get; set; }
        public string career { get; set; }
        public string teachers { get; set; }
        public string group { get; set; }
        public string groupcycle { get; set; }
        public int StudentsThatFit { get; set; }
        public int EnrolledStudents { get; set; }
    }
}
