using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class StudentIncomeScore
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string RegistrationNumber { get; set; }
        public double Score { get; set; }
        public double PossibleScore { get; set; }
        public int Order { get; set; }
        public int TotalStudents { get; set; }
        public Student Student { get; set; }
    }
}
