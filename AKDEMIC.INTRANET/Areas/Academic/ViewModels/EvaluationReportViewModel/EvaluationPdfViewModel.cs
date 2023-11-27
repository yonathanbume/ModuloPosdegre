using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.EvaluationReportViewModel
{
    public class EvaluationPdfViewModel
    {
        public string Img { get; set; }
        public string Term { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public Guid CurriculumId { get; set; }
        public string Curriculum { get; set; }
        public Guid? CareerId { get; set; }
        public string Career { get; set; }
        public string Section { get; set; }
        public string Course { get; set; }
        public string LastGeneratedDate { get; set; }
        public string Status { get; set; }
        public string Teachers { get; set; }
        public string ReceptionDate { get; set; }
        public string CreatedAt { get; set; }
        public string Code { get; set; }
    }
}
