using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class EnrollmentSurveyStudent : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid TermId { get; set; }
        public bool HasComputerOrLaptop { get; set; }
        public bool HasSmartphone { get; set; }
        public bool HasInternet { get; set; }
        public byte InternetConnectionType { get; set; } //1-Datos , 2-ADSL, 3-HFC , 4-Wifi
        public Term Term { get; set; }
        public Student Student { get; set; }
    }
}
