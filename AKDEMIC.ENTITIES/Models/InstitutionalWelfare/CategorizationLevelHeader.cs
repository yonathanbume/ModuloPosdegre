using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class CategorizationLevelHeader
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public ICollection<CategorizationLevel> CategorizationLevels { get; set; }
    }
}
