using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class StudentSummariesTemplate
    {
        public string UserName { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Career { get; set; }
        public decimal WeightedAverageGrade { get; set; }
        public decimal ArithmeticAverageGrade { get; set; }
    } 
}
