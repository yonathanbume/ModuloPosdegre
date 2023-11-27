using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.EvaluationReportViewModels
{
    public class ExtraordinaryEvaluationReportViewModel
    {
        public string Img { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string Course { get; set; }
        public string CourseCode { get; set; }
        public string Teacher { get; set; }
        public decimal Credits { get; set; }
        public string Term { get; set; }
        public string Academicyear { get; set; }
        public int TheoreticalHours { get; set; }
        public int PracticalHours { get; set; }
        public int EffectiveHours { get; set; }
        public string User { get; set; }
        public string ReceptionDate { get; set; }
        public string Code { get; set; }
        public List<ExtraordinarEvalutionDetailViewModel> Students { get; set; }
        public List<string> Committee { get; set; }
    }

    public class ExtraordinarEvalutionDetailViewModel
    {
        public string FullName { get; set; }
        public decimal Grade { get; set; }
        public bool Approved { get; set; }
    }
}
