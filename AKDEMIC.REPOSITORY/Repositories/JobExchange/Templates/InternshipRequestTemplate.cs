using AKDEMIC.CORE.Helpers;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates
{
    public class InternshipRequestTemplate
    {
        public string StudentFullName { get; set; }
        public string ConvalidationType { get; set; }
        public string CareerName { get; set; }
        public string CompanyName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StatusText { get; set; }
    }
}
