using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class CuantityReportTemplate
    {
        public Table Bachiller { get; set; }
        public Table Titulo { get; set; }
        public class Table
        {
            public List<string> Header { get; set; }
            public List<List<string>> Content { get; set; }
            public List<string> Footer { get; set; }
        }
    }
}
