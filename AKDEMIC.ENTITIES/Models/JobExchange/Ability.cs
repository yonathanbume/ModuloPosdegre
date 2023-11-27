using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class Ability : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public int Status { get; set; }

        public int MyProperty { get; set; }

        public string Description { get; set; }

        public ICollection<StudentAbility> StudentAbilities { get; set; }

        public ICollection<JobOfferAbility> JobOfferAbilities { get; set; }
    }
}