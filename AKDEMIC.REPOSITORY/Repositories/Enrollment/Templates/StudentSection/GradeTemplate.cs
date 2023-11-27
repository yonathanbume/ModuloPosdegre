using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class GradesReportTemplate
    {
        public int Sex { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public string SectionCode { get; set; }
    }
    public class FullDataTemplate
    {
        public int Key { get; set; }
        public string Keystring { get; set; }
        //public int Count { get; set; }
        public int AbsoluteFrecuency { get; set; }
        public int CumulativeFrecuency { get; set; }
    }
    //public class Serie
    //{
    //    public string name { get; set; }
    //    public List<int> data { get; set; }
    //}
}
