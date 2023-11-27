using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.AcademicProgram
{
    public class AcademicProgramReportTemplate
    {
        public string Term { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public int Pages { get; set; }
        public List<AcademicProgramGroupTemplate> AcademicPrograms { get; set; }
        public int Total { get; set; }

        public string Logo { get; set; }
        public string SuperiorText { get; set; }
        public string HeaderText { get; set; }
        public string SubheaderText { get; set; }
    }

    public class AcademicProgramGroupTemplate
    {
        public Guid Id { get; set; }
        public int Rowspan { get; set; }

        public string Name { get; set; }

        public int Regular { get; set; }
        public int Observed { get; set; }
        public int Reserved { get; set; }


        public int Third { get; set; }
        public int Fourth { get; set; }
        public int Fifth { get; set; }
        public int Sixth { get; set; }
        public int Seventh { get; set; }
        public int Eighth { get; set; }
        public int Ninth { get; set; }
        public int Tenth { get; set; }
        public int Eleventh { get; set; }
        public int Twelfth { get; set; }
        public int Thirteenth { get; set; }
        public int Fourteenth { get; set; }
        public int Fifteenth { get; set; }





        public int SubTotal { get; set; }
    }
}
