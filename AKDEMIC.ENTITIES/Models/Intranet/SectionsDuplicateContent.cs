using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class SectionsDuplicateContent
    {
        public Guid SectionAId { get; set; }

        public Guid SectionBId { get; set; }

        public Section SectionA { get; set; }

        public Section SectionB { get; set; }
    }
}
