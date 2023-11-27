using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class UserCafeteriaServiceTerm : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? CafeteriaPostulationId { get; set; }
        public Guid StudentId { get; set; }
        public Guid CafeteriaServiceTermId { get; set; }
        public string OwnerId { get; set; }

        public Student Student { get; set; }
        public CafeteriaServiceTerm CafeteriaServiceTerm { get; set; }
        public CafeteriaPostulation CafeteriaPostulation { get; set; }
        public ApplicationUser Owner { get; set; }

        public virtual ICollection<UserCafeteriaDailyAssistance> UserCafeteriaDailyAssistances { get; set; }        
    }
}
