using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentTurn
{
    public class EnrollmentTurnTemplate
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Career { get; set; }

        public string Faculty { get; set; }

        public decimal Credits { get; set; }

        public bool EnableRectification { get; set; }

        public string Observations { get; set; }
    }
}
