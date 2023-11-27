using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.InterestGroup
{
    public class InterestGroupFile : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid InterestGroupId { get; set; }
        public InterestGroup InterestGroup { get; set; }
        public string Name { get; set; }
        public string UrlFile { get; set; }
    }
}
