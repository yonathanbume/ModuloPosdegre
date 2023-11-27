using System;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class CategorizationLevel
    {
        public Guid Id { get; set; }                
        public string Name { get; set; }        
        public int Min { get; set; }
        public int Max { get; set; }
        public Guid CategorizationLevelHeaderId { get; set; }
        public CategorizationLevelHeader CategorizationLevelHeader { get; set; }

    }
}
