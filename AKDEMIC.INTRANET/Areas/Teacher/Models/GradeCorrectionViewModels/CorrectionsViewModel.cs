using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.GradeCorrectionViewModels
{
    public class CorrectionsViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Student { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
        public string Evaluation { get; set; }
        public decimal? OldGrade { get; set; }
        public decimal? Grade { get; set; }
        public int State { get; set; }
        public bool RequestedByStudent { get; set; }
        public string Observations { get; set; }
        public string FilePath { get; set; }
    }
}
