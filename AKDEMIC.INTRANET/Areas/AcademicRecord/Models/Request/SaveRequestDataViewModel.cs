using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Models.Request
{
    public class SaveRequestDataViewModel
    {
        public Guid Id { get; set; }
        public string Student { get; set; }
        public string Username { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public string Semester { get; set; }
        public int Type { get; set; }
        //Para cuadro de meritos
        public double WeightedAverageCumulative { get; set; }

        public List<DetailViewModel> Detail { get; set; }
    }

    public class DetailViewModel
    {
        public string Term { get; set; }
        public int MeritOrder { get; set; }
        public double WeightedAverage { get; set; }
        public double ApprovedCredits { get; set; }
        public int TotalStudents { get; set; }
        public string Observation { get; set; }
        public int UpperFifthTotalStudents { get; set; }
        public int UpperThirdTotalStudents { get; set; }
    }
}
