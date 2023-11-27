using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class GroupChoice : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid VQuestionId { get; set; }

        [RegularExpression(@"[0-9]?[0-9]?[0-9]")]
        public int? Group { get; set; }

        public virtual VQuestion VQuestion { get; set; }
        public virtual ICollection<Choice> Choices { get; set; } = new HashSet<Choice>();
    }
}