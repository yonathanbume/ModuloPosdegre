using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class Template
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? CareerId { get; set; }
        public Career Career { get; set; }
        public string File { get; set; }
    }
}
