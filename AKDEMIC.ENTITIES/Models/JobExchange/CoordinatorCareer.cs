using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class CoordinatorCareer
    {
        public Guid Id { get; set; }
        public Guid CareerId { get; set; }
        public string UserId { get; set; }
        //public Guid SchoolId { get; set; }

        public ApplicationUser User { get; set; }
        public Career Career { get; set; }
        //public School School { get; set; }
    }
}
