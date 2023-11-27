using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ReportTeacherViewModels
{
    public class DetailViewModel
    {
        public string Course { get; set; }
        public string Section { get; set; }
        public Guid SectionId { get; set; }
        public string Teacher { get; set; }
        public List<EvaluationViewModel> Evaluations { get; set; }
        public List<StudentViewModel> Students { get; set; }
    }

    public class EvaluationViewModel
    {
        public Guid Id { get; set; }
        public bool Taken { get; set; }
        public string Name { get; set; }
        public int Percentage { get; set; }
    }

    public class StudentViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Withdrawn { get; set; } = false;
        public List<decimal> Grades { get; set; }
    }
    public class AssistancePdfReportViewModel
    {
        public List<AssistancePdfReportData> ListItems { get; set; }
        public string Img { get; set; }
        public string Teacher { get;  set; }
        public string Course { get;  set; }
    }
    public class AssistancePdfReportData
    {
        public string student { get; set; }
        public int absences { get; set; }
        public int assisted { get; set; }
        public int dictated { get; set; }
        public int maxAbsences { get; set; }
    }
}
