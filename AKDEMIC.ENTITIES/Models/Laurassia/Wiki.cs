using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Wiki : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public Guid SectionId { get; set; }
        public Section Section { get; set; }
        public bool Editable { get; set; }
        public string Description { get; set; }
        public string Plain_Text { get; set; }
        public bool State { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
    }
}
