using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleReportProfile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<ScaleReportProfileDetail> ScaleReportProfileDetails { get; set; }
    }
}
