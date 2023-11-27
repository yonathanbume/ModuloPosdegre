using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SubstituteExam
{
    public class SubstituteExamTemplate
    {
        public Guid id { get; set; }
        public Guid StudentId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public bool isChecked { get; set; }
        public bool Evaluated { get; set; }
        public bool termIsActive { get; set; }
        public string score { get; set; }
        public decimal Grade { get; set; }
        public bool HasGradeCorrection { get; set; }
        public bool HasGradeRecoveryExam { get; set; }
    }
    public class SubstituteExamSectionsTemplate
    {
        public Guid id { get;  set; }
        public Guid courseTermId { get;  set; }
        public string code { get;  set; }
        public string career { get;  set; }
        public string academicYear { get;  set; }
        public string name { get;  set; }
        public string modality { get;  set; }
        public string teachers { get;  set; }
        public string group { get;  set; }
        public string groupcycle { get;  set; }
        public int StudentsThatFit { get; set; }
        public int EnrolledStudents { get; set; }
    }
}
