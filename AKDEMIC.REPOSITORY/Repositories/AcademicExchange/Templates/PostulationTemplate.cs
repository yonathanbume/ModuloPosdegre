using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Templates
{
    public class PostulationTemplate
    {
        public Guid Id { get; set; }
        public string CreatedAtStr { get; set; }
        public string UserFullName { get; set; }
        public string PostulationEmail { get; set; }
        public string ScholarshipName { get; set; }
        public Guid ScholarshipId { get; set; }
        public byte State { get; set; }
        public string StateText {get; set;}
    }
}
