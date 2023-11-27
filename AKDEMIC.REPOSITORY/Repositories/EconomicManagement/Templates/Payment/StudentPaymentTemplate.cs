using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Payment
{
    public class StudentPaymentTemplate
    {
        public string User { get; set; }

        public string FullName { get; set; }

        public string Career { get; set; }

        public int AcademicYear { get; set; }

        public string Invoice { get; set; }

        public decimal Amount { get; set; }

        public string Concept { get; set; }

        public string Type { get; set; }

        public DateTime? Date { get; set; }
    }
}
