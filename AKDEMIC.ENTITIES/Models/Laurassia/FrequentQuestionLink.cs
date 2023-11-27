using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class FrequentQuestionLink : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public Guid FrequentQuestionId { get; set; }
        public FrequentQuestion FrequentQuestion { get; set; }
    }
}
