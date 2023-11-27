using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluation
{
    public class DetailedReport
    {
        public string AcademicDepartment { get; set; }
        public string Faculty { get; set; }
        public string Term { get; set; }
        public int Max { get; set; }

        public string Evaluation { get; set; }

        public string Image { get; set; }
        public List<string> SVGImages { get; set; } = new List<string>();
        
        public List<DataDetails> Details { get; set; }
    }

    public class DetailedReportDetails
    {
        public string name { get; set; }
        public object[] data { get; set; }
    }

    public class DetailedTableReport
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Percentage { get; set; }
    }

    public class DataDetails
    {
        public string Question { get; set; }
        public DetailedReportDetails Details { get; set; }
        public string DetailsSerializeObject => JsonConvert.SerializeObject(Details);
        public List<DetailedTableReport> TableDetails { get; set; }
    }
}
