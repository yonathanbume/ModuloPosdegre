using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class FrequentQuestion : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool State { get; set; }
        public string Title { get; set; }
        public ICollection<FrequentQuestionLink> Links { get; set; }
    }
}
