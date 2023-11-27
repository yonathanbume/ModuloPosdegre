using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class AcademicHistoryAdmissionData
    {
        public Guid StudentId { get; set; }
        public double Score { get; set; }
        public double PossibleScore { get; set; }
        public int Order { get; set; }
        public int TotalStudents { get; set; }
        public string RegistrationNumber { get; set; }
    }
}
