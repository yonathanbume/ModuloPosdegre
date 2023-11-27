using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Class
{
    public class ClassByPlanExcelTemplate
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
        public string Teachers { get; set; }
        public string Semester { get; set; }

        //Clases
        public int Scheduled { get; set; }
        public int Taken { get; set; }
        public decimal Percentage => Scheduled == 0 ? 0 : ((decimal)Taken * 100M / (decimal)Scheduled);
    }
}
