using AKDEMIC.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleReportProfileDetail
    {
        public Guid Id { get; set; }
        public int ScalePdfSectionNumber { get; set; } //AKDEMIC.CORE.Helpers.ConstantHelpers.SCALEPDFSECTIONS
        [NotMapped]
        public string SectionString => ConstantHelpers.SCALEPDFSECTIONS.VALUES.ContainsKey(ScalePdfSectionNumber) ? ConstantHelpers.SCALEPDFSECTIONS.VALUES[ScalePdfSectionNumber] : "-";
        public Guid ScaleReportProfileId { get; set; }
        public ScaleReportProfile ScaleReportProfile { get; set; }
    }
}
