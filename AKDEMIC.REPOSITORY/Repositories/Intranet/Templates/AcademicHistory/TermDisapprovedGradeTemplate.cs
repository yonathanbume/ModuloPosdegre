using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory
{
    public class TermDisapprovedGradeTemplate
    {
        public string Term { get; set; }

        public int Year { get; set; }

        public string Number { get; set; }

        public double FirstTry { get; set; }

        public double SecondTry { get; set; }

        public double ThirdOrMoreTry { get; set; }
    }
}
